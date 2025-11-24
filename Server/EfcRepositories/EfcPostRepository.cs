using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext _context;

    public EfcPostRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        EntityEntry<Post> entry = await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(Post post)
    {
        if (post is null) throw new ArgumentNullException();
        
        if (!await _context.Posts.AnyAsync(p => p.PostId == post.PostId))
        {
            throw new Exception($"Post with id: {post.PostId} not found");
        }

        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        
    }

    public async Task DeleteAsync(int id)
    {
        var existingPost = await _context.Posts.SingleOrDefaultAsync(p => p.PostId == id);
        
        if (existingPost is null) throw new Exception($"Post with id: {id} not found");

        _context.Posts.Remove(existingPost);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllFromSubforumAsync(int subforumId)
    {
        var posts = await _context.Posts.Where(p => p.InSubforum.SubforumId == subforumId).ToListAsync();
        foreach (var post in posts)
        {
            _context.Posts.Remove(post);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllFromUserAsync(int userId)
    {
        var posts = await _context.Posts.Where(p => p.Author.UserId == userId).ToListAsync();
        foreach (var post in posts)
        {
            _context.Posts.Remove(post);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.InSubforum)
            .Include(p => p.CommentedOnPost)
            .SingleOrDefaultAsync(p => p.PostId == id);
        if (post is null) throw new Exception($"Post with id: {id} not found");
        
        Console.WriteLine(post.Author.Username);
        
        return post;
    }

    public IQueryable<Post> GetMany()
    {
        return _context.Posts.AsQueryable();
    }
}