using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.ChatBotApiModel;

public class MessagePostApiModel : AuditFields
{
    public Guid? messageId { get; set; }
    public Guid? conversationId { get; set; }
    public string content { get; set; }
    public string? role { get; set; }
}
