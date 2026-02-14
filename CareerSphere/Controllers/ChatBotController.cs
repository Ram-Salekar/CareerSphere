using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;
using Microsoft.AspNetCore.Mvc;

namespace CareerSphere.Controllers
{
    [ApiController]
    public class ChatBotController : Controller
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly IFileReader _fileReader;

        public ChatBotController(IOpenRouterService openRouterService, IFileReader fileReader)
        {
            _openRouterService = openRouterService;
            _fileReader = fileReader;

        }

        [HttpPost("api/chatbot/respond")]
        public async Task<IActionResult> GetChatBotResponse([FromBody] string message)
        {
            var response = await _openRouterService.GetResponseModel(message);
            return Ok(response);
        }

        [HttpPost("api/agent/resumeagent")]
        public async Task<IActionResult> GetAgentPrompt(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF allowed.");

            string extractedText;

            using (var stream = file.OpenReadStream())
            {
                extractedText = _fileReader.ExtractTextFromPdf(stream);
            }

            var response = await _openRouterService.ResumeAnalyzingAgent(extractedText);
            return Ok(response);
            
        }

        [HttpPost("api/pdfData")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF allowed.");

            string extractedText;

            using (var stream = file.OpenReadStream())
            {
                extractedText = _fileReader.ExtractTextFromPdf(stream);
            }

            return Ok(extractedText);
        }
    }
}
