using AutoMapper;
using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Models.MessageTableModel;


namespace CareerSphere.Mapper
{
    public class MessageAndConversationMapper : Profile

    {
        public MessageAndConversationMapper()
        {
            CreateMap<MessagePostApiModel, MessageModel>().ReverseMap();
            CreateMap<MessageResponseApiModel, MessageModel>().ReverseMap();
            CreateMap<ConversationPostApiModel, Conversation>().ReverseMap();
            CreateMap<ConversationResponseApiModel, Conversation>().ReverseMap();
          
        }
    }
}
