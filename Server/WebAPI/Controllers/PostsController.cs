using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _posts;
    private readonly IReactionRepository _reactions;
    private readonly IUserRepository _users;
    private readonly ISubforumRepository _subforums;

    public PostsController(IPostRepository posts, IReactionRepository reactions, IUserRepository users, ISubforumRepository subforums)
    {
        _posts = posts;
        _reactions = reactions;
        _users = users;
        _subforums = subforums;
    }
    
    private async Task<Post> DtoPostToEntity(PostDTO post)
    {
        var onPost = post.CommentedOnPostId is not null && post.CommentedOnPostId.HasValue
            ? await _posts.GetSingleAsync(post.CommentedOnPostId.Value)
            : null;
        User author = await _users.GetSingleAsyncById(post.AuthorId);
        Subforum subforum = await _subforums.GetSingleAsync(post.SubforumId);

        return new()
        {
            Title = post.Title ?? "",
            Author = author,
            Content = post.Content ?? "",
            InSubforum = subforum,
            PostId = post.PostId,
            CommentedOnPost = onPost
        };
    }

    private PostDTO EntityPostToDto(Post post)
    {
        return new PostDTO()
        {
            Title = post.Title,
            AuthorId = post.Author.UserId,
            CommentedOnPostId = post.CommentedOnPost?.PostId,
            Content = post.Content,
            SubforumId = post.InSubforum.SubforumId,
            PostId = post.PostId
        };
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDTO>> Create([FromBody] PostDTO post)
    {
        Post createdPost = await _posts.AddAsync(await DtoPostToEntity(post));
        
        return Created($"/Posts/{createdPost.PostId}", EntityPostToDto(createdPost));
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<PostDTO>> Update([FromBody] PostDTO post)
    {
        await _posts.UpdateAsync(await DtoPostToEntity(post));
        
        return Ok("Updated Post");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetSingle([FromRoute] int id)
    {
        PostDTO post = EntityPostToDto(await _posts.GetSingleAsync(id));

        return Ok(post);
    }

    [HttpGet("getmany")]
    public async Task<ActionResult<List<PostDTO>>> GetAll(  [FromQuery] string? title, 
                                                            [FromQuery] int? authorId, 
                                                            [FromQuery] int? commentedOnPostId,
                                                            [FromQuery] int? subforumId)
    {
        var posts = _posts.GetMany()
            .Include(p => p.Author)
            .Include(p => p.InSubforum)
            .Include(p => p.CommentedOnPost).AsQueryable();
        
        if (title != null) posts = posts.Where(p => p.Title != null && p.Title.ToLower().Contains(title.ToLower()));
        if (authorId != null) posts = posts.Where(p => p.Author.UserId == authorId);
        if (commentedOnPostId != null) posts = posts.Where(p => p.CommentedOnPost!.PostId == commentedOnPostId);
        if (subforumId != null) posts = posts.Where(p => p.InSubforum.SubforumId == subforumId);
        
        var retList = new List<PostDTO>();
        foreach (var post in posts)
        {
            retList.Add(EntityPostToDto(post));
        }
        
        return Ok(retList);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PostDTO>> Delete([FromRoute] int id)
    {
        try
        {
            await _posts.DeleteAsync(id);

            return Ok("Post deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{id}/react")]
    public async Task<ActionResult<PostDTO>> Like([FromRoute] int id, [FromBody] ReactionDTO reaction)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes

            await _reactions.AddAsync(new()
            {
                ByUser = await _users.GetSingleAsyncById(reaction.ByUserId),
                OnPost = await _posts.GetSingleAsync(reaction.PostId),
                Type = reaction.Type,
            });

            return Ok($"Reacted '{reaction.Type}' to post");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}/react")]
    public async Task<ActionResult<PostDTO>> Dislike([FromRoute] int id, [FromBody] ReactionDTO reaction)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes
            
            var targetReaction = _reactions.GetMany()
                .Where(r => r.OnPost.PostId == reaction.PostId)
                .Where(r => r.Type == reaction.Type)
                .Include(r => r.ByUser)
                .FirstOrDefault(r => r.ByUser.UserId == reaction.ByUserId);
            
            if (targetReaction == null) throw new InvalidOperationException("No matching reaction found");
            
            await _reactions.DeleteAsync(targetReaction);

            return Ok($"Removed reaction '{reaction.Type}' from post");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/reactions")]
    public async Task<ActionResult<List<ReactionDTO>>> GetReactions([FromRoute] int id)
    {    
        var reactions = await _reactions.GetMany()
            .Where(r => r.OnPost.PostId == id)
            .Include(r => r.ByUser)
            .Include(r => r.OnPost)
            .ToListAsync();
        
        var reactionDtos = reactions.Select(r => new ReactionDTO
            {
                ReactionId = r.ReactionId,
                Type = r.Type,
                ByUserId = r.ByUser.UserId,
                PostId = r.OnPost.PostId
            })
            .ToList();

        return Ok(reactionDtos);
    }
}