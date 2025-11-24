using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcSubforumRepository : ISubforumRepository
{
    private readonly AppContext _context;

    public EfcSubforumRepository(AppContext context)
    {
        _context = context;
    }
    public async Task<Subforum> AddAsync(Subforum subforum)
    {
        EntityEntry<Subforum> entry = await _context.Subforums.AddAsync(subforum);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(Subforum subforum)
    {
        if (subforum is null) throw new ArgumentNullException(nameof(subforum));

        if (!await _context.Subforums.AnyAsync(p => p.SubforumId == subforum.SubforumId))
        {
            throw new Exception($"Subforum with id: {subforum.SubforumId} not found");
        }

        _context.Subforums.Update(subforum);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existingSubforum = await _context.Subforums.FirstOrDefaultAsync(p => p.SubforumId == id);
        if (existingSubforum == null) throw new Exception($"Subforum with id: {id} not found");
        
        _context.Subforums.Remove(existingSubforum);
        await _context.SaveChangesAsync();
    }

    public async Task<Subforum> GetSingleAsync(int id)
    {
        var existingSubforum = await _context.Subforums.Include(s => s.Moderator).FirstOrDefaultAsync(p => p.SubforumId == id);
        if (existingSubforum == null) throw new Exception($"Subforum with id: {id} not found");
        
        return existingSubforum;
    }

    public IQueryable<Subforum> GetMany()
    {
        return _context.Subforums.Include(s => s.Moderator).AsQueryable();
    }
}