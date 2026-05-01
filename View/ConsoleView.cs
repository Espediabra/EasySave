public class ConsoleView
{
    private readonly MainViewModel _viewModel;

    public ConsoleView(MainViewModel viewModel)
    {
        _viewModel = viewModel;
    }
    public void Start()
    {
        Console.WriteLine("EasySave");
        Console.WriteLine("1.run saves");

        var choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                _viewModel.ExecuteJobs();
                break;

        }
    }
}