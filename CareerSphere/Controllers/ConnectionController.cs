using CareerSphere.Repository.ConnectionRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    public class ConnectionController : Controller
    {
        private readonly IConnectionRepo _connectionRepo;

        public ConnectionController(IConnectionRepo connectionRepo)
        {
            _connectionRepo = connectionRepo;
        }

        [HttpPost("api/connections/request")]
        public async Task<IActionResult> SendConnectionRequest([FromBody]Guid receiverId)
        {
            var senderId = Guid.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));

            var result = await _connectionRepo.SendConnectionRequestAsync(senderId, receiverId);

            if (!result)
            {
                return BadRequest("Cannot send connection request to yourself.");
            }

            return Ok("Connection request sent successfully.");

        }
        [HttpDelete("api/connections/request")]
        public async Task<IActionResult> RemoveConnection([FromQuery] Guid receiverId)
        {
            var senderId = Guid.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));

            var result = await _connectionRepo.RemoveConnectionAsync(senderId, receiverId);

            if (!result)
            {
                return BadRequest("Cannot remove connection.");
            }

            return Ok("Connection removed successfully.");
        }
    }
}
