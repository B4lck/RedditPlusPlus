namespace RepositoryContracts;

using Entities;

public interface ISubforumRepository
{
    /// <summary>
    /// Add subforum to repository async
    /// </summary>
    /// <param name="subforum">Subforum to add</param>
    /// <returns>The added Subforum</returns>
    Task<Subforum> AddAsync(Subforum subforum);
    
    /// <summary>
    /// Updates Subforum in repository async
    /// </summary>
    /// <param name="subforum">Subforum to delete</param>
    /// <exception cref="Exception">If Subforum is not found</exception>
    /// <returns>Nothing</returns>
    Task UpdateAsync(Subforum subforum);
    
    /// <summary>
    /// Delete Subforum in repository async
    /// </summary>
    /// <param name="id">ID of the subforum</param>
    /// <exception cref="Exception">If Subforum is not found</exception>
    /// <returns>Nothing</returns>
    Task DeleteAsync(int id);
    
    /// <summary>
    /// Get a single Subforum from repository async
    /// </summary>
    /// <param name="id">ID of the subforum</param>
    /// <exception cref="Exception">If Subforum is not found</exception>
    /// <returns>Subforum with matching ID</returns>
    Task<Subforum> GetSingleAsync(int id);
    
    /// <summary>
    /// Get all Subforums from repository async
    /// </summary>
    /// <returns>All Subforums in repository</returns>
    IQueryable<Subforum> GetMany();   
}