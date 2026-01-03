using CareerSphere.Models.UserTableModel;
using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.PostApiModels
{
    public class PostCreateApiModel  
    {
       
        public Guid UserId { get; set; }
        public string? description { get; set; }
        public string? contentImageUrl { get; set; }
    }
}
