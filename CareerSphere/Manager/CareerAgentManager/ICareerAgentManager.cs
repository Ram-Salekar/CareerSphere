using CareerSphere.ApiModels;
using CareerSphere.ApiModels.ReActAgentModels;

namespace CareerSphere.Manager.CareerAgentManager
{
    public interface ICareerAgentManager
    {
        Task<AgentUserResponse> RunAgentAsync(
             AgentQueryRequest request,
             Guid userId);

    }
}
