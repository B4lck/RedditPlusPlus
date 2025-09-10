using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class CreateUserView : IView
{
    private ViewHandler _viewHandler;
    private IUserRepository _userRepository;
    
    public CreateUserView(ViewHandler viewHandler, IUserRepository userRepository)
    {
        this._viewHandler = viewHandler;
        this._userRepository = userRepository;
    }
    
    public void Display()
    {
        Console.WriteLine("-- Create User --");
        Console.WriteLine("To create a new user, enter a username and password, seperate them with spaces");
        Console.WriteLine("Example: [username] [password]");
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

                    if (_userRepository.GetMany().Any(u => u.Username == username))
                        break;

                    _userRepository.AddAsync(new User()
                    {
                        Username = username,
                        Password = password
                    });
                    Console.WriteLine($"User {username} added");
                    break;
            }
            
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.CreateUser);
        }
    }
}