using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcReactionRepository : IReactionRepository
{
    private readonly AppContext _context;

    public EfcReactionRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        EntityEntry<Reaction> entry = await _context.Reactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task DeleteAsync(Reaction reaction)
    {
        var exiting = await _context.Reactions.SingleOrDefaultAsync(r => r.ReactionId == reaction.ReactionId);
        if (exiting == null) throw new Exception($"Reaction with id {reaction.ReactionId} not found");
        
        _context.Reactions.Remove(exiting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(Post post)
    {
        var reactions = await _context.Reactions.Where(r => r.OnPost.PostId == post.PostId).ToListAsync();
        foreach (var reaction in reactions)
        {
            _context.Reactions.Remove(reaction);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(User user)
    {
        var reactions = await _context.Reactions.Where(r => r.ByUser.UserId == user.UserId).ToListAsync();
        foreach (var reaction in reactions)
        {
            _context.Reactions.Remove(reaction);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTotalOfTypeAsync(Post post, string type)
    {
        return await _context.Reactions.Where(r => r.OnPost.PostId == post.PostId && r.Type == type).CountAsync();
    }

    public IQueryable<Reaction> GetMany()
    {
        return _context.Reactions.AsQueryable();
    }
}