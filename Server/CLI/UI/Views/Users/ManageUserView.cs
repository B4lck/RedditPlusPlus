using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class ManageUserView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly IUserRepository _userRepository;
    
    public ManageUserView(ViewHandler viewHandler, IUserRepository userRepository)
    {
        _viewHandler = viewHandler;
        _userRepository = userRepository;
    }
    
    public void Display()
    {
        Console.WriteLine("-- Manage User --");
        Console.WriteLine("To change a password, write username and new password");
        Console.WriteLine("Example: [Username] [New Password]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("-----------------");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    _viewHandler.GoToMainMenu();
                    break;
                default:
                    var args = input.Split(' ');
                    if (args.Length != 2)
                        throw new ArgumentException("Invalid args");
                    var username = args[0];
                    var password = args[1];

                    _userRepository.UpdateAsync(new User { Username = username, Password = password });
                    Console.WriteLine($"User {username} changed password to {password}");
                    break;
            }
            
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            _viewHandler.GoToView(Views.ManageUser);
        }
    }
}