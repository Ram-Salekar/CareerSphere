using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.ChatBotApiModel
{
    public class MessageResponseApiModel : AuditFields
    {
        public string content { get; set; }
        public string role { get; set; }
    }
}
