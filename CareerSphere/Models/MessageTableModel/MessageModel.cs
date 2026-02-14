using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.Models.MessageTableModel
{
    public class MessageModel : AuditFields
    {
        public Guid messageId { get; set; }
        public Guid conversationId { get; set; }
        public string content { get; set; }
        public string role { get; set; }
        public Conversation conversation { get; set; }
        
    }
}
