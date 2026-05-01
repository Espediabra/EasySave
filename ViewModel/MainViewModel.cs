public class MainViewModel
{
    private readonly ExecuteJobsCommand _executeJobsCommand;

    public MainViewModel(ExecuteJobsCommand executeJobsCommand)
    {
        _executeJobsCommand = executeJobsCommand;
    }

    public void ExecuteJobs()
    {
        _executeJobsCommand.Execute();
    }
}