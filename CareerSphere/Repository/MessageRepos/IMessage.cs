using CareerSphere.ApiModels.ChatBotApiModel;

namespace CareerSphere.Repository.MessageRepos
{
    public interface IMessage
    {
        public Task<bool>PostMessage(MessagePostApiModel messagePostApiModel);
        public Task<List<MessageResponseApiModel>> viewMessagesByConeversationId(Guid ConversationId);
        public Task<List<MessageResponseApiModel>> Last5messages(Guid conversationId);
    }
}
