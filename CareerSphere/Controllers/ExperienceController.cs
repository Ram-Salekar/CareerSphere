using CareerSphere.ApiModels.ExperienceApiModel;
using CareerSphere.Repository.ExperienceRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : Controller
    {
        private readonly IExperienceRepo _experienceRepo;

        public ExperienceController(IExperienceRepo experienceRepo)
        {
            _experienceRepo = experienceRepo;
        }

       
        private Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
            return userId;
        }

      
        [HttpPost  ("/api/addExp")]
        public async Task<IActionResult> AddExperience([FromBody] ExperiencePostApimodel model)
        {
            var userId = GetUserId();

            var result = await _experienceRepo.AddExperience(model, userId);

            if (!result)
                return BadRequest("Unable to add experience");

            return Ok("Experience added successfully");
        }

       
        [HttpGet]
        public async Task<IActionResult> GetMyExperiences()
        {
            var userId = GetUserId();

            var experiences = await _experienceRepo.GetAllExperienceByUserId(userId);

            return Ok(experiences);
        }

       
        [HttpGet("{experienceId}")]
        public async Task<IActionResult> GetExperienceById(Guid experienceId)
        {
            var experience = await _experienceRepo.GetExperienceById(experienceId);

            if (experience == null)
                return NotFound("Experience not found");

            return Ok(experience);
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateExperience([FromBody] ExperiencePostApimodel model)
        {
            var userId = GetUserId();

            var result = await _experienceRepo.UpdateExperience(model, userId);

            if (!result)
                return NotFound("Experience not found or update failed");

            return Ok("Experience updated successfully");
        }

        
        [HttpDelete("{experienceId}")]
        public async Task<IActionResult> DeleteExperience(Guid experienceId)
        {
            var result = await _experienceRepo.DeleteExperience(experienceId);

            if (!result)
                return NotFound("Experience not found");

            return Ok("Experience deleted successfully");
        }
    }
}
