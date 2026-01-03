using Microsoft.AspNetCore.Mvc;
using CareerSphere.ApiModels.UsersApiModels;
using Microsoft.AspNetCore.Authorization;

namespace CareerSphere.Controllers
{
    
    [ApiController]
    public class UserController : Controller
    {
        
        private readonly Repository.UserRepoFolder.IUser _userRepo;

        public UserController(Repository.UserRepoFolder.IUser userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("api/users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userRepo.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("api/users/{id}/username")]
        public async Task<IActionResult> GetUserNameAsync(Guid id)
        {
            var username = await _userRepo.GetUserName(id);
            if (username == null)
            {
                return NotFound();
            }
            return Ok(username);
        }

        [HttpPost("api/users")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateApiModel userCreateApiModel)
        {
            var user = await _userRepo.CreateUserAsync(userCreateApiModel);
            return Ok(user);
        }

        [HttpGet("api/users/emailorusername")]
        public async Task<IActionResult> GetUserByEmailOrUserName([FromQuery] string emailOrUsername)
        {
            var user = await _userRepo.GetUserByEmailOrUserName(emailOrUsername);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        
    }
}
