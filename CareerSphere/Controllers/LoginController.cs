using CareerSphere.ApiModels.AuthModels;
using CareerSphere.ApiModels.UsersApiModels;
using CareerSphere.Repository.UserRepoFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CareerSphere.Controllers
{
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUser _userRepo;

        public LoginController(IUser userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost ("api/login")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult> getToken(LoginApiModel detail)
        {
            var token = await _userRepo.getToken(detail);
            return Ok(token);
        }

        [HttpPost("api/register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateApiModel userCreateApiModel)
        {
            var user = await _userRepo.CreateUserAsync(userCreateApiModel);
            return Ok(user);
        }
      

    }
}
