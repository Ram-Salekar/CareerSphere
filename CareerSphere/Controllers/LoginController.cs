using CareerSphere.ApiModels.AuthModels;
using CareerSphere.Repository.UserRepoFolder;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost ("login")]
        public async Task<IActionResult> getToken(LoginApiModel detail)
        {
            var token = await _userRepo.getToken(detail);
            return Ok(token);
        }
    }
}
