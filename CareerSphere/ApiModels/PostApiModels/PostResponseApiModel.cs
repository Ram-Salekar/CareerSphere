using CareerSphere.Models.UserTableModel;
using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.PostApiModels
{
    public class PostResponseApiModel : AuditFields
    {
        public Guid id { get; set; }
        public Guid UserId { get; set; }
        public string? description { get; set; }
        public string? contentImageUrl { get; set; }
        
    }
}
