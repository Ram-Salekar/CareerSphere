using AutoMapper;
using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Data;
using CareerSphere.Models.MessageTableModel;
using CareerSphere.ApiModels.ChatBotModels;
using Microsoft.EntityFrameworkCore;

namespace CareerSphere.Repository.MessageRepos
{
    public class MessageRepo : IMessage
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepo(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> PostMessage(MessagePostApiModel messagePostApiModel)
        {
            MessageModel messageEntity = _mapper.Map<MessageModel>(messagePostApiModel);

            _context.Messages.Add(messageEntity);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<MessageResponseApiModel>> viewMessagesByConeversationId(Guid ConversationId)
        {
            var messages = await _context.Messages
               .Where(m => m.conversationId == ConversationId)
               .ToListAsync();

            return _mapper.Map<List<MessageResponseApiModel>>(messages);
        }

        public async Task<List<MessageResponseApiModel>> Last5messages(Guid conversationId)
        {
            var messages = await _context.Messages
                .Where(m => m.conversationId == conversationId)
                .OrderByDescending(m => m.createdAt)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<MessageResponseApiModel>>(messages);

        }
    }
}
