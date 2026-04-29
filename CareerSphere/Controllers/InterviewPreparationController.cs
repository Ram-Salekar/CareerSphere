using CareerSphere.ApiModels.InterviewPrepModels;
using CareerSphere.Manager.InterviewPreparationManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    public class InterviewPreparationController : Controller
    {
        private readonly IInterviewPreparationManager _interviewPreparationManager;

        public InterviewPreparationController(IInterviewPreparationManager interviewPreparationManager)
        {
            _interviewPreparationManager = interviewPreparationManager;

        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCoverLetter(
            [FromForm] CoverLetterRequestModel request)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _interviewPreparationManager
                .GenerateCoverLetterAsync(request, userId);

            return Ok(result);
        }

    }
}
