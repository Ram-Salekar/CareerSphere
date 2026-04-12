using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.ChatBotModels;
using CareerSphere.ApiModels.JSearchApiModels;
using CareerSphere.Manager.JobManager;

namespace CareerSphere.Services.AiChatBotService
{
    public interface IOpenRouterService
    {
        public Task<MessageResponseApiModel>GetResponseModel(string message, Guid conversationId);
        public Task<string> ResumeAnalyzingAgent(string resume);
        public Task<string> GetJobList(string resume, List<JobListing>joblist);
        public Task<JSearchParamsResult> ExtractJSearchParamsAsync(string resumeText);
    }
}
