using System.ComponentModel;
using System.Runtime.CompilerServices;
using CLI.UI.Views.Forums;
using CLI.UI.Views.Posts;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI.Views;

public class ViewHandler
{
    private IPostRepository _posts;
    private IUserRepository _users;
    private ISubforumRepository _subforums;
    private IReactionRepository _reactions;
    
    
    private MainMenuView _mainMenuView;
    
    private CreateUserView _createUserView;
    private ManageUserView _manageUserView;
    private DeleteUserView _deleteUserView;
    private ListUserView _listUserView;
    
    private CreateSubforumView _createSubforumView;
    private ManageSubforumView _manageSubforumView;
    private ListSubforumView _listSubforumView;
    private DeleteSubforumView _deleteSubforumView;
    private OpenSubforumView _openSubforumView;
    
    private OpenPostView _openPostView;

    public ViewState ViewState { get; } = new ViewState();

    public ViewHandler (IPostRepository posts, 
                        IUserRepository users, 
                        ISubforumRepository subforums, 
                        IReactionRepository reactions)
    {
        _posts = posts;
        _users = users;
        _subforums = subforums;
        _reactions = reactions;

        _mainMenuView = new MainMenuView(this);
        
        _createUserView = new CreateUserView(this, _users);
        _manageUserView = new ManageUserView(this, _users);
        _deleteUserView = new DeleteUserView(this, _users, _posts);
        _listUserView = new ListUserView(this, _users);

        _createSubforumView = new CreateSubforumView(this, _subforums, _users);
        _manageSubforumView = new ManageSubforumView(this, _subforums);
        _deleteSubforumView = new DeleteSubforumView(this, _subforums, _posts);
        _listSubforumView = new ListSubforumView(this, _subforums);
        _openSubforumView = new OpenSubforumView(this, _subforums, _posts, _users);

        _openPostView = new OpenPostView(this, _subforums, _posts, _users, _reactions);
    }

    public void GoToView(Views viewName)
    {
        IView view;
        switch (viewName)
        {
            case Views.CreateUser:
                view = _createUserView;
                break;
            case Views.ManageUser:
                view = _manageUserView;
                break;
            case Views.DeleteUser:
                view = _deleteUserView;
                break;
            case Views.ListUsers:
                view = _listUserView;
                break;
            case Views.CreateSubforum:
                view = _createSubforumView;
                break;
            case Views.ManageSubforum:
                view = _manageSubforumView;
                break;
            case Views.DeleteSubforum:
                view = _deleteSubforumView;
                break;
            case Views.ListSubforums:
                view = _listSubforumView;
                break;
            case Views.OpenSubforum:
                view = _openSubforumView;
                break;
            case Views.OpenPost:
                view = _openPostView;
                break;
            default:
                view = _mainMenuView;
                break;
        }
        ShowView(view);
    }
    
    private void ShowView(IView view)
    {
        Console.Clear();
        view.Display();
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input is null)
                continue;
            
            view.HandleInput(input);
            break;
        }
    }

    public void GoToMainMenu()
    {
        ShowView(new MainMenuView(this));
    }
}

public enum Views
{
    MainMenu,
    
    CreateUser,
    ManageUser,
    DeleteUser,
    ListUsers,
    
    CreateSubforum,
    ManageSubforum,
    DeleteSubforum,
    ListSubforums,
    OpenSubforum,
    
    OpenPost
}

public class ViewState
{
    public Subforum? CurrentSubforum { get; set; }
    public Post? CurrentPost { get; set; }
}