using CareerSphere.Models.UserTableModel;
using CareerSphere.Utility.UtilityModels;

namespace CareerSphere.Models.EducationTableModel
{
    public class Education 
    {
        public Guid userId { get; set; }
        public User user { get; set; }
        public Guid educationId { get; set; }
        public string degree { get; set; }
        public string Institution { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
