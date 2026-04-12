namespace CareerSphere.ApiModels.JSearchApiModels
{
    public class JobListing
    {
        public string JobId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ApplyLink { get; set; }
        public string EmploymentType { get; set; }
        public string PostedAt { get; set; }
        public List<string> RequiredSkills { get; set; }
    }
}
