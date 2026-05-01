class Program
{
    static void Main(string[] args)
    {
        var fileService = new FileService();
        var logService = new LogService();
        var stateService = new StateService();

        var backupService = new BackupService(fileService, logService, stateService);

        var executeJobsCommand = new ExecuteJobsCommand(backupService);

        var MainViewModel = new MainViewModel(executeJobsCommand);

        var view = new ConsoleView(MainViewModel);

        view.Start(); 

    }
}