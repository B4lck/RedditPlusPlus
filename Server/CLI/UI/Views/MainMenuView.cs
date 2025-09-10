using Entities;

namespace CLI.UI.Views;

public class MainMenuView : IView
{
    private ViewHandler handler;

    public MainMenuView(ViewHandler handler)
    {
        this.handler = handler;
    }
    
    public void Display()
    {
        Console.WriteLine("Welcome to the Reddit Plus Plus");
        Console.WriteLine("-------------------------------");
        Console.WriteLine("What page do you want to go to?");
        Console.WriteLine(" - Create User");
        Console.WriteLine(" - Manage User");
        Console.WriteLine(" - Delete User");
        Console.WriteLine(" - List Users");
        Console.WriteLine("");
        Console.WriteLine(" - Create subforum");
        Console.WriteLine(" - Manage subforum");
        Console.WriteLine(" - Delete subforum");
        Console.WriteLine(" - List subforums");
    }

    public void HandleInput(string input)
    {
        switch (input.ToLower())
        {
            case "create user":
                handler.GoToView(Views.CreateUser);
                break;
            case "manage user":
                handler.GoToView(Views.ManageUser);
                break;
            case "delete user":
                handler.GoToView(Views.DeleteUser);
                break;
            case "list users":
                handler.GoToView(Views.ListUsers);
                break;
            case "create subforum":
                handler.GoToView(Views.CreateSubforum);
                break;
            case "manage subforum":
                handler.GoToView(Views.ManageSubforum);
                break;
            case "delete subforum":
                handler.GoToView(Views.DeleteSubforum);
                break;
            case "list subforums":
                handler.GoToView(Views.ListSubforums);
                break;
            default:
                handler.GoToMainMenu();
                break;
        }
    }
}