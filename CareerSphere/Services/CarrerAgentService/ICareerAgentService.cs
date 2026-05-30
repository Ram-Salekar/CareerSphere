using CareerSphere.ApiModels;
using System.Globalization;

namespace CareerSphere.Services.CarrerAgentService
{
    public interface ICareerAgentService
    {
        Task<AgentResponse> RunAgentAsync(string? usergoal , string? resumeText);
    }
}
