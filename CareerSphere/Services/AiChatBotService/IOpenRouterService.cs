using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.ChatBotModels;

namespace CareerSphere.Services.AiChatBotService
{
    public interface IOpenRouterService
    {
        public Task<MessageResponseApiModel>GetResponseModel(string message, Guid conversationId);
        public Task<string> ResumeAnalyzingAgent(string resume);
    }
}
