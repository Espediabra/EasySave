using EasySave.Models;
using EasySave.Services;

namespace EasySave.Strategies;

/// <summary>
/// Full backup strategy: copies all files from source to target.
/// </summary>
public class FullBackupStrategy : IBackupStrategy
{
    private readonly FileService _fileService = new();
    private readonly LogService _logService = LogService.GetInstance();
    private readonly StateService _stateService = new();

    public void Execute(BackupJob job)
    {
        if (!Directory.Exists(job.SourcePath))
            throw new DirectoryNotFoundException($"Source not found: {job.SourcePath}");

        var files = Directory.GetFiles(job.SourcePath, "*", SearchOption.AllDirectories);

        long totalSize = files.Sum(f => new FileInfo(f).Length);

        // J'aurais pu faire : 
        //foreach (var file in files)
        //    totalSize += new FileInfo(file).Length;

        var state = new State
        {
            BackupName = job.Name,
            Status = "Active",
            TotalFiles = files.Length,
            RemainingFiles = files.Length,
            TotalSize = totalSize,
            RemainingSize = totalSize
        };

        foreach (var sourceFile in files)
        {
            var relativePath = Path.GetRelativePath(job.SourcePath, sourceFile);
            var targetFile = Path.Combine(job.TargetPath, relativePath);

            // Le ! indique au compilateur que je suis sûr que GetDirectoryName ne retournera pas null, car sourceFile est un chemin valide.
            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

            // Démarrer le chronomètre pour mesurer le temps de transfert
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _fileService.CopyFile(sourceFile, targetFile);

                stopwatch.Stop();

                var fileSize = new FileInfo(sourceFile).Length;

                // Update state
                state.Timestamp = DateTime.Now;
                state.CurrentSourceFile = sourceFile;
                state.CurrentTargetFile = targetFile;
                state.RemainingFiles--;
                state.RemainingSize -= fileSize;

                _stateService.Update(state);

                // Log success
                _logService.CreateLog(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = job.Name,
                    SourcePath = sourceFile,
                    TargetPath = targetFile,
                    FileSize = fileSize,
                    TransferTime = stopwatch.ElapsedMilliseconds
                });
            }
            catch
            {
                stopwatch.Stop();

                _logService.CreateLog(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = job.Name,
                    SourcePath = sourceFile,
                    TargetPath = targetFile,
                    FileSize = 0,
                    // Je mets TransferTime à -1 pour respecter la convention d'erreur de transfert
                    TransferTime = -1
                });
            }
        }

        state.Status = "Completed";
        _stateService.Update(state);
    }
}