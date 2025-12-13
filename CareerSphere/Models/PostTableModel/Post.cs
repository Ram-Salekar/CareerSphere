using CareerSphere.Utility.UtilityModels;
using CareerSphere.Models.UserTableModel;
namespace CareerSphere.Models.PostTableModel
{
    public class Post : AuditFields
    {
        public Guid id { get; set; }
        public Guid UserId { get; set; }
        public User user { get; set; }
        public string? description { get; set; }
        public string? contentImageUrl { get; set; }
        
    }
}
