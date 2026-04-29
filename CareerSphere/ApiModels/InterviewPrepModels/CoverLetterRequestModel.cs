namespace CareerSphere.ApiModels.InterviewPrepModels
{
    public class CoverLetterRequestModel
    {
        public IFormFile? Resume { get; set; }
        public string JobDescription { get; set; }
    }
}
