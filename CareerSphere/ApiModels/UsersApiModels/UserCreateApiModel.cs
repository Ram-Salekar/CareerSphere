using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.ApiModels.UsersApiModels
{
    public class UserCreateApiModel : AuditFields
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }

        public string? profileImageUrl { get; set; }
        public string? about { get; set; }
        public string? header { get; set; }

        public string username { get; set; }
        public string email { get; set; }


    }
}
