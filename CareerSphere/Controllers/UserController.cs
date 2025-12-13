using Microsoft.AspNetCore.Mvc;

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
        
    }
}
