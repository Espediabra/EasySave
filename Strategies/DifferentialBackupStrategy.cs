using EasySave.Models;
using EasySave.Services;

namespace EasySave.Strategies;

/// <summary>
/// Differential backup: copies only new or modified files.
/// </summary>
public class DifferentialBackupStrategy : IBackupStrategy
{
    private readonly FileService _fileService = new();
    private readonly LogService _logService = LogService.GetInstance();
    private readonly StateService _stateService = new();

    public void Execute(BackupJob job)
    {
        if (!Directory.Exists(job.SourcePath))
            throw new DirectoryNotFoundException($"Source not found: {job.SourcePath}");

        var sourceFiles = Directory.GetFiles(job.SourcePath, "*", SearchOption.AllDirectories);

        // La seule différence avec le full backup, c'est que je filtre les fichiers à copier en fonction de leur date de modification par rapport à la cible.
        var filesToCopy = sourceFiles.Where(sourceFile =>
        {
            var relativePath = Path.GetRelativePath(job.SourcePath, sourceFile);
            var targetFile = Path.Combine(job.TargetPath, relativePath);

            // Cas 1 : le fichier n'existe pas dans la cible, je le copie.
            if (!File.Exists(targetFile))
                return true;

            // Cas 2 : le fichier existe dans la cible, je compare les dates de modification.
            // Si le fichier source est plus récent, je le copie.
            return File.GetLastWriteTimeUtc(sourceFile) > File.GetLastWriteTimeUtc(targetFile);
        }).ToList();

        long totalSize = filesToCopy.Sum(f => new FileInfo(f).Length);

        var state = new State
        {
            BackupName = job.Name,
            Status = "Active",
            TotalFiles = filesToCopy.Count,
            RemainingFiles = filesToCopy.Count,
            TotalSize = totalSize,
            RemainingSize = totalSize
        };

        foreach (var sourceFile in filesToCopy)
        {
            var relativePath = Path.GetRelativePath(job.SourcePath, sourceFile);
            var targetFile = Path.Combine(job.TargetPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _fileService.CopyFile(sourceFile, targetFile);

                stopwatch.Stop();

                var fileSize = new FileInfo(sourceFile).Length;

                state.Timestamp = DateTime.Now;
                state.CurrentSourceFile = sourceFile;
                state.CurrentTargetFile = targetFile;
                state.RemainingFiles--;
                state.RemainingSize -= fileSize;

                _stateService.Update(state);

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
                    TransferTime = -1
                });
            }
        }

        state.Status = "Completed";
        _stateService.Update(state);
    }
}