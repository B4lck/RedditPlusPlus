using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class ListUserView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly IUserRepository _userRepository;
    
    public ListUserView(ViewHandler viewHandler, IUserRepository userRepository)
    {
        _viewHandler = viewHandler;
        _userRepository = userRepository;
    }
    
    public void Display()
    {
        Console.WriteLine("-- Users listed --");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("------------------");

        var users = _userRepository.GetMany();
        foreach (var user in users)
        {
            Console.WriteLine($"- Username: {user.Username} - Id: {user.UserId}");
        }

    }

    public void HandleInput(string input)
    {
        // Den er ligeglad om hvad du skriver
        _viewHandler.GoToMainMenu();
    }
}