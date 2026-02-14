using CareerSphere.Models.MessageTableModel;
using CareerSphere.Models.UserTableModel;
using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.ChatBotApiModel
{
    public class ConversationPostApiModel : AuditFields
    {
        public Guid conversationId { get; set; }
        public Guid userId { get; set; }
        public string Title { get; set; }
    }
}
