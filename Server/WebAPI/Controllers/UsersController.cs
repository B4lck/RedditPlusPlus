using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public UsersController(IUserRepository userRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    private User DtoUserToEntity(UserDTO user)
    {
        return new()
        {
            UserId = user.UserId,
            Username = user.Username
        };
    }

    private UserDTO EntityUserToDto(User user)
    {
        return new()
        {
            UserId = user.UserId,
            Username = user.Username
        };
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody]CreateUserDTO userDto)
    {
        User createdUser = await _userRepository.AddAsync(new () {Username = userDto.Username, Password = userDto.Password});
        
        return Created($"/Users/{createdUser.UserId}", EntityUserToDto(createdUser));
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetMany([FromQuery] string? username)
    {
        var users = _userRepository.GetMany();

        if (username != null)
        {
            users = users.Where(u => u.Username.ToLower().Contains(username.ToLower()));
        }
        
        return Ok(await users.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById([FromRoute] int id)
    {
        return Ok(await _userRepository.GetSingleAsyncById(id));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDTO>> Delete([FromRoute] int id)
    {
        await _postRepository.DeleteAllFromUserAsync(id);
        
        await _userRepository.DeleteAsync(id);

        return Ok("User deleted");
    }

    [HttpPost("update")]
    public async Task<ActionResult<UserDTO>> Update([FromBody] UserDTO userDto)
    {
        await _userRepository.UpdateAsync(DtoUserToEntity(userDto));
        
        return Ok("User updated");
    }
}