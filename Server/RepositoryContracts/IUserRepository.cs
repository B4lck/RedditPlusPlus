namespace RepositoryContracts;

using Entities;

public interface IUserRepository
{
    /// <summary>
    /// Add User to repository async
    /// </summary>
    /// <param name="user">User to add</param>
    /// <returns>User added</returns>
    Task<User> AddAsync(User user);
    
    /// <summary>
    /// Update User in repository async
    /// </summary>
    /// <param name="user">User to update</param>
    /// <exception cref="Exception">If User is not found</exception>
    /// <returns>Nothing</returns>
    Task UpdateAsync(User user);
    
    /// <summary>
    /// Delete User in repository async
    /// </summary>
    /// <param name="id">ID of User to delete</param>
    /// <exception cref="Exception">If User is not found</exception>
    /// <returns>Nothing</returns>
    Task DeleteAsync(int id);
    
    /// <summary>
    /// Get a single User by ID async
    /// </summary>
    /// <param name="id">ID of User</param>
    /// <exception cref="Exception">If User is not found</exception>
    /// <returns>User with matching ID</returns>
    Task<User> GetSingleAsyncById(int id);
    
    /// <summary>
    /// Get a single User by Username async
    /// </summary>
    /// <param name="username">Username of User</param>
    /// <exception cref="Exception">If User is not found</exception>
    /// <returns>User with matching Username</returns>
    Task<User> GetSingleAsyncByUsername(string username);
    
    /// <summary>
    /// Get all users from repository async
    /// </summary>
    /// <returns>All Users in repository</returns>
    IQueryable<User> GetMany();
    
    /// <summary>
    /// Verify if User has matching Username and Passwords
    /// </summary>
    /// <param name="username">Username of User</param>
    /// <param name="password">Password of User</param>
    /// <returns>If User has matching credentials</returns>
    Task<Boolean> VerifyCredentialsAsync(string username, string password);
}