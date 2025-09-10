using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Forums;

public class CreateSubforumView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    private readonly IUserRepository _userRepository;
    
    public CreateSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository, IUserRepository userRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
        _userRepository = userRepository;
    }


    public void Display()
    {
        Console.WriteLine("-- Create Subforum --");
        Console.WriteLine("To create a new subforum, type id of moderator and name of subforum");
        Console.WriteLine("Example: [id] [name]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("---------------------");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    break;
                default:
                    var args = input.Split(" ");
                    var moderatorId = int.Parse(args[0]);
                    var name = input.Substring(args[0].Length + 1);

                    // Hvis der allerede er et subforum med dette navn
                    if (_subforumRepository.GetMany().Any(s => s.Name.ToLower() == name.ToLower()))
                        break;
                    // Hvis brugeren ikke findes
                    if (!_userRepository.GetMany().Any(u => u.UserId == moderatorId))
                        break;

                    _subforumRepository.AddAsync(new Subforum()
                    {
                        ModeratorId = moderatorId,
                        Name = name
                    });
                    
                    break;
            }
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.CreateSubforum);
        }
    }
}