using RepositoryContracts;

namespace CLI.UI.Views.Forums;

public class ListSubforumView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    
    public ListSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
    }


    public void Display()
    {
        Console.WriteLine("-- Subforums listed --");
        Console.WriteLine("To open af subforum, type the ID of the subforum");
        Console.WriteLine("Example: [ID]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("----------------------");

        var subforums = _subforumRepository.GetMany();
        foreach (var subforum in subforums)
        {
            Console.WriteLine($"- Name: {subforum.Name} - Id: {subforum.SubforumId} - Mod Id: {subforum.ModeratorId}");
        }
    }

    public async void HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    break;
                default:
                    int sfId = int.Parse(input);
                    
                    var subforum = await _subforumRepository.GetSingleAsync(sfId);
                    _viewHandler.ViewState.CurrentSubforum = subforum;
                    
                    _viewHandler.GoToView(Views.OpenSubforum);
                    
                    break;
            }
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.ListSubforums);
            Console.WriteLine(e.Message);
        }
    }
}