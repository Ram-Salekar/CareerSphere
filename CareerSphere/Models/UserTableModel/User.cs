using CareerSphere.Utility.UtilityModels;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Models.ConnectionTableModel;
using CareerSphere.Models.MessageTableModel;
using CareerSphere.Models.EducationTableModel;

namespace CareerSphere.Models.UserTableModel
{
    public class User : AuditFields
    {
       
        public Guid id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? profileImageUrl { get; set; }
        public string? about { get; set; }
        public string? header {get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }

        public IEnumerable<Post>? Posts { get; set; } = new List<Post>();
        public IEnumerable<Connection>? followers { get; set; } = new List<Connection>();
        public IEnumerable<Connection>? followings { get; set; } = new List<Connection>();
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
        public ICollection<Education>Educations { get; set; } = new List<Education>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    }
}
