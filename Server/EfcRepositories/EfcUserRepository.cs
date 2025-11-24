using System.Globalization;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _context;

    public EfcUserRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> entry = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(User user)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));

        var resUser = await _context.Users.SingleOrDefaultAsync(p => p.UserId == user.UserId);
        if (resUser is null)
        {
            throw new Exception($"User with id: {user.UserId} not found");
        }

        resUser.Username = user.Username;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Users.SingleOrDefaultAsync(p => p.UserId == id);
        if (entity == null)
        {
            throw new Exception($"User with id: {id} not found");
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsyncById(int id)
    {
        var entity = await _context.Users.SingleOrDefaultAsync(p => p.UserId == id);
        if (entity == null)
        {
            throw new Exception($"User with id: {id} not found");
        }

        return entity;
    }

    public async Task<User> GetSingleAsyncByUsername(string username)
    {
        var entity = await _context.Users.SingleOrDefaultAsync(p => p.Username == username);
        if (entity == null)
        {
            throw new Exception($"User with username: {username} not found");
        }

        return entity;
    }

    public IQueryable<User> GetMany()
    {
        return _context.Users.AsQueryable();
    }

    public async Task<bool> VerifyCredentialsAsync(string username, string password)
    {
        return await _context.Users.AnyAsync(p => p.Username == username && p.Password == password);
    }
}