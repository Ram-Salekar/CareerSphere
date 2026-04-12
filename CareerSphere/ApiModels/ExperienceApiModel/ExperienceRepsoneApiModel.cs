using CareerSphere.Models.UserTableModel;

namespace CareerSphere.ApiModels.ExperienceApiModel
{
    public class ExperienceRepsoneApiModel
    {
        
        public string jobTitle { get; set; }
        public string company { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string description { get; set; }
        
    }
}
