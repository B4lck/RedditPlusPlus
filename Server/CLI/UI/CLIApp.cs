using System.Runtime.InteropServices;
using CLI.UI.Views;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CLIApp
{
    private IPostRepository _posts;
    private IUserRepository _users;
    private ISubforumRepository _subforums;
    private IReactionRepository _reactions;

    private ViewHandler handler;

    public CLIApp(IPostRepository posts, IUserRepository users, ISubforumRepository subforums, IReactionRepository reactions)
    {
        _posts = posts;
        _users = users;
        _subforums = subforums;
        _reactions = reactions;
        
        handler = new ViewHandler(posts, users, subforums, reactions);
        
        // Dummy data
        _users.AddAsync(new User()
        {
            Username = "admin",
            Password = "admin",
        });
        _users.AddAsync(new User()
        {
            Username = "test1",
            Password = "123",
        });
        _users.AddAsync(new User()
        {
            Username = "test2",
            Password = "123",
        });

        _subforums.AddAsync(new Subforum()
        {
            Name = "r/Danmark",
            ModeratorId = 1,
        });
        _subforums.AddAsync(new Subforum()
        {
            Name = "r/Sverige",
            ModeratorId = 2,
        });
        _subforums.AddAsync(new Subforum()
        {
            Name = "r/Norge",
            ModeratorId = 3,
        });

        _posts.AddAsync(new Post()
        {
            AuthorId = 1,
            Title = "Vi overtager Sverige",
            Content = "Vi skal i krig, vi myrder svenskerne!!",
            SubforumId = 1
        });
        _posts.AddAsync(new Post()
        {
            AuthorId = 2,
            Title = "Danskarna kommer",
            Content = "Alla vet att Danmark är coolare än Sverige, så vi borde bara ge upp.",
            SubforumId = 2
        });
        _posts.AddAsync(new Post()
        {
            AuthorId = 3,
            Content = "Håller helt med",
            SubforumId = 2,
            CommentedOnPostId = 2
        });
        _posts.AddAsync(new Post()
        {
            AuthorId = 1,
            Content = "Jeg kommer og spiser dig. Din dumme dumme svensker.",
            SubforumId = 2,
            CommentedOnPostId = 2
        });
        _posts.AddAsync(new Post()
        {
            AuthorId = 3,
            Title = "Danskene tar over Sverige, LOL!",
            Content = "Ingen liker dem uansett",
            SubforumId = 3,
        });
    }

    public Task StartAsync()
    {
        handler.GoToView(Views.Views.MainMenu);
        
        return Task.CompletedTask;
    }
}