public class ExecuteJobsCommand
{
    private readonly BackupService _backupService;

    public ExecuteJobsCommand(BackupService backupService)
    {
        _backupService = backupService;
    }

    public void Execute()
    {
        _backupService.RunAllJobs();
    }
}