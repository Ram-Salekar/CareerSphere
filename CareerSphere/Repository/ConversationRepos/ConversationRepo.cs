using AutoMapper;
using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Data;
using CareerSphere.Models.MessageTableModel;
using Microsoft.EntityFrameworkCore;

namespace CareerSphere.Repository.ConversationRepos
{
    public class ConversationRepo : IConversation
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper mapper;

        public ConversationRepo(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            this.mapper = mapper;
        }

         public async Task<bool> PostConversation(ConversationPostApiModel conversation)
         {
            Conversation conversationEntity = mapper.Map<Conversation>(conversation);
            _appDbContext.Add(conversationEntity);
            await _appDbContext.SaveChangesAsync();
             return true;

         }

         public async Task<List<ConversationResponseApiModel>> ViewConversationsByUserId(Guid userId)
            {
                var conversations = await _appDbContext.Conversations
                    .Where(c => c.userId == userId)
                    .ToListAsync();

            return mapper.Map<List<ConversationResponseApiModel>>(conversations);
            }
    }
}
