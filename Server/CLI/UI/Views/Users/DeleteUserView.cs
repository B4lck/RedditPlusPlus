using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class DeleteUserView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    
    public DeleteUserView(ViewHandler viewHandler, IUserRepository userRepository, IPostRepository postRepository)
    {
        _viewHandler = viewHandler;
        _userRepository = userRepository;
        _postRepository = postRepository;
    }


    public void Display()
    {
        Console.WriteLine("-- Delete User --");
        Console.WriteLine("To delete user, write id of user");
        Console.WriteLine("Example: [id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("-----------------");
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
                    int userId = int.Parse(input);
                    _userRepository.DeleteAsync(userId);
                    _postRepository.DeleteAllFromUserAsync(userId);
                    break;
            }
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.DeleteUser);
        }
    }
}