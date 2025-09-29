using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Posts;

public class OpenPostView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReactionRepository _reactionRepository;
    
    public OpenPostView(ViewHandler viewHandler, ISubforumRepository subforumRepository, IPostRepository postRepository, IUserRepository userRepository, IReactionRepository reactionRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _reactionRepository = reactionRepository;
    }
    public async void Display()
    {
        if (_viewHandler.ViewState.CurrentPost is null)
        {
            _viewHandler.GoToMainMenu();
            return;
        }
        
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("To comment, type comment, user id of author and the content of the comment");
        Console.WriteLine("Example: comment [id] [content]");
        Console.WriteLine("To react, type react, user id of author and like or dislike");
        Console.WriteLine("Example: react [id] like/dislike");
        Console.WriteLine("To remove, type remove");
        Console.WriteLine("Example: remove");
        Console.WriteLine("To remove a comment, type remove and id of comment");
        Console.WriteLine("Example: remove [comment id]");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine($"-- {_viewHandler.ViewState.CurrentPost.Title} --");
        Console.WriteLine($"Author: {_userRepository.GetSingleAsync(_viewHandler.ViewState.CurrentPost.AuthorId).Result.Username}");
        Console.WriteLine("-");
        Console.WriteLine(_viewHandler.ViewState.CurrentPost.Content);
        Console.WriteLine("-----------------------------");
        
        // reactions
        var reactionsOfPost = _reactionRepository.GetMany().ToList().FindAll(r => r.PostId == _viewHandler.ViewState.CurrentPost.PostId);
        var likes = reactionsOfPost.FindAll(r => r.Type == "like");
        var dislikes = reactionsOfPost.FindAll(r => r.Type == "dislike");
        Console.WriteLine($"Likes: {likes.Count} - Dislikes: {dislikes.Count}");
        Console.WriteLine("-----------------------------");

        
        // comments
        var commentsOnCurrentPost = (await _postRepository.GetMany()).ToList().FindAll(c => c.CommentedOnPostId == _viewHandler.ViewState.CurrentPost.PostId);
        foreach (var comment in commentsOnCurrentPost)
        {
            Console.WriteLine($"    -- ID: {comment.PostId} | By: {_userRepository.GetSingleAsync(comment.AuthorId).Result.Username}");
            Console.WriteLine($"        {comment.Content}");
            Console.WriteLine("");
        }
    }

    public void HandleInput(string input)
    {
        try
        {
            var loweredInput = input.ToLower();
            var loweredSplitInput = loweredInput.Split(" ");
            switch (loweredSplitInput[0])
            {
                case "exit":
                    _viewHandler.GoToView(Views.OpenSubforum);
                    break;
                case "comment":
                    var userId = int.Parse(loweredSplitInput[1]);
                    var content = input.Substring(loweredSplitInput[0].Length + loweredSplitInput[1].Length + 2);
                    _postRepository.AddAsync(new Post()
                    {
                        AuthorId = userId,
                        Content = content,
                        SubforumId = _viewHandler.ViewState.CurrentPost.SubforumId,
                        CommentedOnPostId = _viewHandler.ViewState.CurrentPost.PostId,
                    });
                    _viewHandler.GoToView(Views.OpenPost);
                    break;
                case "react":
                    userId = int.Parse(loweredSplitInput[1]);
                    
                    var reaction = loweredSplitInput[2];
                    if (reaction != "like" && reaction != "dislike")
                        throw new Exception("Invalid reaction");
                    
                    _reactionRepository.AddAsync(new Reaction()
                    {
                        ByUserId = userId,
                        PostId = _viewHandler.ViewState.CurrentPost.PostId,
                        Type = reaction
                    });
                    _viewHandler.GoToView(Views.OpenPost);
                    break;
                case "remove":
                    if (loweredSplitInput.Length == 1)
                    {
                        _postRepository.DeleteAsync(_viewHandler.ViewState.CurrentPost.PostId);
                        _viewHandler.GoToView(Views.OpenSubforum);
                    }
                    else
                    {
                        var commentId = int.Parse(loweredSplitInput[1]);
                        _postRepository.DeleteAsync(commentId);
                        
                        _viewHandler.GoToView(Views.OpenPost);
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.OpenPost);
        }
    }
}