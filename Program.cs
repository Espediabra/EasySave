using EasySave.Services;

class Program
{
    static void Main(string[] args)
    {
        var backupService = new BackupService();

        // Je teste simplement le Job 1 pour voir s'il s'exécute correctement 
        backupService.RunJobs(new List<int> { 1 });

        Console.WriteLine("Test finished.");
    }
}