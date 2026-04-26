using EasySave.Models;
using EasySave.Factories;

namespace EasySave.Services;

/// <summary>
/// Central service that orchestrates backup execution.
/// </summary>
public class BackupService
{
    private readonly BackupStrategyFactory _factory = new();

    // Temporary in-memory jobs (to be replaced by config later)
    private readonly List<BackupJob> _jobs = new()
    {
        new BackupJob("Job1", "C:/TestData/Source1", "C:/TestData/Target1", BackupType.Differential),
        new BackupJob("Job2", "C:/TestData/Source2", "C:/TestData/Target2", BackupType.Differential),
        new BackupJob("Job3", "C:/TestData/Source3", "C:/TestData/Target3", BackupType.Full),
        new BackupJob("Job4", "C:/TestData/Source4", "C:/TestData/Target4", BackupType.Differential),
        new BackupJob("Job5", "C:/TestData/Source5", "C:/TestData/Target5", BackupType.Full)
    };

    /// <summary>
    /// Run selected jobs sequentially.
    /// </summary>
    public void RunJobs(List<int> ids)
    {
        foreach (var id in ids)
        {
            if (id < 1 || id > _jobs.Count)
                continue;

            var job = _jobs[id - 1];

            var strategy = _factory.Create(job.Type);

            strategy.Execute(job);
        }
    }
}