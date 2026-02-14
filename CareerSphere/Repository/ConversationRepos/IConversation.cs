using CareerSphere.ApiModels.ChatBotApiModel;

namespace CareerSphere.Repository.ConversationRepos
{
    public interface IConversation
    {
        public Task<bool> PostConversation(ConversationPostApiModel conversation);
        public Task<List<ConversationResponseApiModel>> ViewConversationsByUserId(Guid userId);
    }
}
