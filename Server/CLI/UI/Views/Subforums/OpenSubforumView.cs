using System.Runtime.CompilerServices;
using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Forums;

public class OpenSubforumView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    
    public OpenSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }
    
    public void Display()
    {
        // Null tjek
        if (_viewHandler.ViewState.CurrentSubforum is null)
        {
            _viewHandler.GoToMainMenu();
            Console.WriteLine("No subforum selected");
            return;
        }
        
        Console.WriteLine($"-- {_viewHandler.ViewState.CurrentSubforum.Name} --");
        Console.WriteLine($"Moderated by: {_userRepository.GetSingleAsync(_viewHandler.ViewState.CurrentSubforum.ModeratorId).Result.Username}");
        Console.WriteLine("");
        Console.WriteLine("To create a post, write create, the userid of the author, and the title of the post, press enter and then type the content of the post");
        Console.WriteLine("Example: create [Author id] [Title]");
        Console.WriteLine("Example: [content]");
        Console.WriteLine("");
        Console.WriteLine("To open a post, enter open and the id of the post");
        Console.WriteLine("Example: open [Post id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("_________________________________________________");

        // Henter alle posts fra dette forum
        var posts = _postRepository.GetMany().ToList()
            .FindAll(p => p.SubforumId == _viewHandler.ViewState.CurrentSubforum.SubforumId);

        var comments = posts.FindAll(p => p.CommentedOnPostId != null);
        posts.RemoveAll(p => comments.Contains(p));

        if (posts.Count == 0)
            Console.WriteLine("No one has posted yet, be the first! Type create [author id] [title]");
        
        foreach (var post in posts)
        {
            Console.WriteLine($"-- {post.Title} --");
            Console.WriteLine($"By: {_userRepository.GetSingleAsync(post.AuthorId).Result.Username} - Post ID: {post.PostId}");
            Console.WriteLine("-");
            Console.WriteLine($"{post.Content}");
            Console.WriteLine("-");
            Console.WriteLine("");
        }
    }

    public void HandleInput(string input)
    {
        var inputLowerCase = input.ToLower();
        try
        {
            if (inputLowerCase == "exit")
            {
                _viewHandler.GoToView(Views.ListSubforums);
                return;
            }
            
            var splitInput = inputLowerCase.Split(" ");
            switch (splitInput[0])
            {
                case "create":
                    var authorId = int.Parse(splitInput[1]);
                    var substringCutIndex = splitInput[0].Length + splitInput[1].Length + 2;
                    var title = input.Substring(substringCutIndex);
                    string content;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"-- Write post: {title} --");
                        Console.WriteLine("Type 'cancel' to exit.");
                        Console.WriteLine("__________________________");
                        content = Console.ReadLine();

                        if (content == "cancel")
                        {
                            _viewHandler.GoToView(Views.ListSubforums);
                            break;
                        }

                        if (content is not null)
                            break;
                    }

                    _postRepository.AddAsync(new Post()
                    {
                        AuthorId = authorId,
                        Content = content,
                        SubforumId = _viewHandler.ViewState.CurrentSubforum.SubforumId,
                        Title = title
                    });
                    break;
                case "open":
                    var postId = int.Parse(splitInput[1]);
                    _viewHandler.ViewState.CurrentPost = _postRepository.GetSingleAsync(postId).Result;
                    _viewHandler.GoToView(Views.OpenPost);
                    break;
            }
            _viewHandler.GoToView(Views.OpenSubforum);
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.OpenSubforum);
        }
    }
}