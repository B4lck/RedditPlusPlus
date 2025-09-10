using RepositoryContracts;

namespace CLI.UI.Views.Forums;

public class DeleteSubforumView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    private readonly IPostRepository _postRepository;
    
    public DeleteSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository, IPostRepository postRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
        _postRepository = postRepository;
    }
    public void Display()
    {
        Console.WriteLine("-- Delete Subforum --");
        Console.WriteLine("To delete subforum, write id of subforum");
        Console.WriteLine("Example: [id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("---------------------");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input)
            {
                case "exit":
                    break;
                default:
                    int subforumId = int.Parse(input);
                    _subforumRepository.DeleteAsync(subforumId);
                    _postRepository.DeleteAllFromSubforumAsync(subforumId);
                    break;
            }
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.DeleteSubforum);
        }
    }
}