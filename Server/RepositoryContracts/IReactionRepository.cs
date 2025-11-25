namespace RepositoryContracts;

using Entities;

public interface IReactionRepository
{
    /// <summary>
    /// Add Reaction to repository async
    /// </summary>
    /// <param name="reaction">Reaction to add</param>
    /// <returns>Added Reaction</returns>
    Task<Reaction> AddAsync(Reaction reaction);
    
    /// <summary>
    /// Delete Reaction from repository async
    /// </summary>
    /// <param name="reaction">Reaction to remove</param>
    /// <exception cref="Exception">If Reaction is not found</exception>
    /// <returns>Nothing</returns>
    Task DeleteAsync(Reaction reaction);
    
    /// <summary>
    /// Delete all Reactions from specified Post async
    /// </summary>
    /// <param name="post">Post to remove all Reactions from</param>
    /// <returns>Nothing</returns>
    Task DeleteAllAsync(Post post);
    
    /// <summary>
    /// Delete all Reactions made by specified User async
    /// </summary>
    /// <param name="user">User to remove all Reactions from</param>
    /// <returns>Nothing</returns>
    Task DeleteAllAsync(User user);
    
    /// <summary>
    /// Get count of reaction type from specified post async
    /// </summary>
    /// <param name="post">From post</param>
    /// <param name="type">Type of reaction</param>
    /// <returns>Count</returns>
    Task<int> GetTotalOfTypeAsync(Post post, string type);
    
    /// <summary>
    /// Get all Reactions in Repository
    /// </summary>
    /// <returns>All Reactions</returns>
    IQueryable<Reaction> GetMany();
}