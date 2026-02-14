using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Manager.ChatBotManager;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatBotController : Controller
    {
       private readonly IChatBotManager _chatBotManager;

        public ChatBotController(IChatBotManager chatBotManager)
        {
            _chatBotManager = chatBotManager;

        }

        [HttpPost("api/chatbot/respond")]
        public async Task<IActionResult> GetChatBotResponse([FromBody] MessagePostApiModel message)
        {
            Guid userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            var response = await _chatBotManager.GetChatBotResponse(message,userId);
            return Ok(response);
        }

        [HttpPost("api/agent/resumeagent")]
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

      
    }
}
