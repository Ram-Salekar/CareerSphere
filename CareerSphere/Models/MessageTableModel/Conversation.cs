using CareerSphere.Models.UserTableModel;
using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.Models.MessageTableModel
{
    public class Conversation : AuditFields
    {
        public Guid conversationId { get; set; }
        public Guid userId { get; set; }
        public ICollection<MessageModel> messages { get; set; } = new List<MessageModel>();
        public User User { get; set; }
        public string Title { get; set; }
    }
}
