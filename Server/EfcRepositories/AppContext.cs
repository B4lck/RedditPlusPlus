using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Subforum> Subforums => Set<Subforum>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source = C:\Users\Nikol\RiderProjects\RedditPlusPlus\Server\EfcRepositories\app.db");
    }
}