using Microsoft.AspNetCore.Mvc;
using CareerSphere.ApiModels.UsersApiModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CareerSphere.Controllers
{
    [Authorize]  
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

        [HttpGet("api/users/LoginedUser")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var claimsPrincipal = User;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepo.GetUserByIdAsync(Guid.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("api/users/editdetails")]
        public async Task <IActionResult> updateCurrentUser([FromBody]UserCreateApiModel user)
        {
            Guid id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _userRepo.UpdateUserAsync(id,user);
            return Ok(result);
        }
        
    }
}
