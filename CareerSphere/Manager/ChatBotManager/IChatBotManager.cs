using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.JSearchApiModels;

namespace CareerSphere.Manager.ChatBotManager
{
    public interface IChatBotManager
    {
        public Task<MessageResponseApiModel> GetChatBotResponse(MessagePostApiModel message,Guid userId);
        public Task<string> ResumeAnalyzingAgent(IFormFile resume, Guid userId);
        public Task <List<JobListing>> GetJobByResume(IFormFile resume, Guid userId);
    }
}
