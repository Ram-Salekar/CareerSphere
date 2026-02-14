using CareerSphere.Utility.ChatBotModels;

namespace CareerSphere.Services.AiChatBotService
{
    public interface IOpenRouterService
    {
        public Task<string>GetResponseModel(string message);
        public Task<string> ResumeAnalyzingAgent(string resume);
    }
}
