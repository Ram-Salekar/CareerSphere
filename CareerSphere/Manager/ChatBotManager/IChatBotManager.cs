using CareerSphere.ApiModels.ChatBotApiModel;

namespace CareerSphere.Manager.ChatBotManager
{
    public interface IChatBotManager
    {
        public Task<MessageResponseApiModel> GetChatBotResponse(MessagePostApiModel message,Guid userId);
        public Task<string> ResumeAnalyzingAgent(IFormFile resume, Guid userId);
    }
}
