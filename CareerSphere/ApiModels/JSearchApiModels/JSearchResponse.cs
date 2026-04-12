using System.Text.Json.Serialization;

namespace CareerSphere.ApiModels.JSearchApiModels
{
    public class JSearchResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public List<JSearchJob> Data { get; set; }
    }

    public class JSearchJob
    {
        [JsonPropertyName("job_id")]
        public string JobId { get; set; }

        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; }

        [JsonPropertyName("employer_name")]
        public string EmployerName { get; set; }

        [JsonPropertyName("job_city")]
        public string JobCity { get; set; }

        [JsonPropertyName("job_country")]
        public string JobCountry { get; set; }

        [JsonPropertyName("job_description")]
        public string JobDescription { get; set; }

        [JsonPropertyName("job_apply_link")]
        public string JobApplyLink { get; set; }

        [JsonPropertyName("job_employment_type")]
        public string JobEmploymentType { get; set; }

        [JsonPropertyName("job_posted_at_datetime_utc")]
        public string JobPostedAt { get; set; }

        [JsonPropertyName("job_required_skills")]
        public List<string> JobRequiredSkills { get; set; }

        [JsonPropertyName("job_experience_in_place_of_education")]
        public bool? ExperienceRequired { get; set; }
    }
}
