using CareerSphere.ApiModels.ReActAgentModels;
using CareerSphere.Manager.CareerAgentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CareerSphere.Controllers
{
    [ApiController]
    [Authorize]
    public class CareerAgentController : Controller
    {
        private readonly ICareerAgentManager careerAgentManager;

        public CareerAgentController(ICareerAgentManager careerAgentManager)
        {
            this.careerAgentManager = careerAgentManager;
        }

        [HttpPost("api/analyze")]
        public async Task<IActionResult> AnalyzeResume([FromForm] AgentQueryRequest agentQuery)
        {
         return (IActionResult)await careerAgentManager.RunAgentAsync(agentQuery, Guid.NewGuid());
        }
    }
}
