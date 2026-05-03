using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Manager.ChatBotManager;
using CareerSphere.Manager.JserviceManager;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatBotController : Controller
    {
       private readonly IChatBotManager _chatBotManager;
       

        public ChatBotController(IChatBotManager chatBotManager, IJservice jservice)
        {
            _chatBotManager = chatBotManager;
           

        }

        [HttpPost("api/chatbot/respond")]
        [EnableRateLimiting("AiPolicy")]
        public async Task<IActionResult> GetChatBotResponse([FromBody] MessagePostApiModel message)
        {
            Guid userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            var response = await _chatBotManager.GetChatBotResponse(message,userId);
            return Ok(response);
        }

        [HttpPost("api/agent/resumeagent")]
        [EnableRateLimiting("AiPolicy")]
        public async Task<IActionResult> GetAgentPrompt(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF allowed.");

            Guid userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            var response = await _chatBotManager.ResumeAnalyzingAgent(file,userId);
            return Ok(response);
            
        }

        [HttpPost("api/agent/getjoblistbyresume")]
        [EnableRateLimiting("AiPolicy")]
        public async Task<IActionResult>GetJobList(IFormFile file)
        {
            var result = await _chatBotManager.GetJobByResume(file, Guid.Empty);
            return Ok(result);
        }

    }
}
