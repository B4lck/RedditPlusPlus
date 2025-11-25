namespace RepositoryContracts;

using Entities;

public interface IPostRepository
{
    /// <summary>
    /// Add Post to repository async
    /// </summary>
    /// <param name="post">Post to add</param>
    /// <returns>The added Post</returns>
    Task<Post> AddAsync(Post post);
    
    /// <summary>
    /// Update Post in repository async
    /// </summary>
    /// <param name="post">Post to update</param>
    /// <exception cref="Exception">If Post is not found</exception>
    /// <returns>Nothing</returns>
    Task UpdateAsync(Post post);
    
    /// <summary>
    /// Delete Post in repository async
    /// </summary>
    /// <param name="id">ID of post</param>
    /// <exception cref="Exception">If Post is not found</exception>
    /// <returns>Nothing</returns>
    Task DeleteAsync(int id);
    
    
    /// <summary>
    /// Delete all Posts from specified Subforum async
    /// </summary>
    /// <param name="subforumId">ID of the Subforum</param>
    /// <returns>Nothing</returns>
    Task DeleteAllFromSubforumAsync(int subforumId);
    
    /// <summary>
    /// Delete all Posts made by specfied User async
    /// </summary>
    /// <param name="userId">ID of User</param>
    /// <returns>Nothing</returns>
    Task DeleteAllFromUserAsync(int userId);
    
    /// <summary>
    /// Get Post from ID async
    /// </summary>
    /// <param name="id">ID of Post</param>
    /// <exception cref="Exception">If Post is not found</exception>
    /// <returns>Post with matching ID</returns>
    Task<Post> GetSingleAsync(int id);
    
    /// <summary>
    /// Get all Posts
    /// </summary>
    /// <returns>All posts in repository</returns>
    IQueryable<Post> GetMany();
}