using CareerSphere.ApiModels.JSearchApiModels;
using System.Text.Json;

namespace CareerSphere.Manager.JserviceManager
{
    public class JService : IJservice
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public JService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<JobListing>> FetchJobsAsync(
            string role,
            string location,
            List<string> skills, int exp)
        {
            try
            {
                var apiKey = _configuration["JSearch:ApiKey"];
                var baseUrl = _configuration["JSearch:BaseUrl"];
                var resultsPerPage = _configuration["JSearch:ResultsPerPage"] ?? "10";

                // Build smart query from role + top 2 skills
                var topSkills = skills != null && skills.Count > 0
                    ? string.Join(" ", skills.Take(2))
                    : string.Empty;

                var expLabel = GetExperienceLabel(exp);

                // Build query with role + skills + experience label
                var query = $"{expLabel} {role} {topSkills}".Trim();
                var fullLocation = string.IsNullOrWhiteSpace(location) ? "India" : location;

                var url = $"{baseUrl}/search" +
                          $"?query={Uri.EscapeDataString(query)}" +
                          $"&location={Uri.EscapeDataString(fullLocation)}" +
                          $"&page=1&num_pages=1" +
                          $"&results_per_page={resultsPerPage}";



                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "jsearch.p.rapidapi.com");




                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    throw new Exception($"JSearch failed: {error}");
                }

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<JSearchResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data == null || result.Data.Count == 0)
                {

                    return new List<JobListing>();
                }

                // Map to clean internal model
                return result.Data.Select(j => new JobListing
                {
                    JobId = j.JobId,
                    Title = j.JobTitle,
                    Company = j.EmployerName,
                    Location = j.JobCity ?? fullLocation,
                    Description = j.JobDescription,
                    ApplyLink = j.JobApplyLink,
                    EmploymentType = j.JobEmploymentType,
                    PostedAt = j.JobPostedAt,
                    RequiredSkills = j.JobRequiredSkills ?? new List<string>()
                }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private string GetExperienceLabel(int years)
        {
            return years switch
            {
                0 => "fresher entry level",
                1 => "junior entry level",
                2 or 3 => "junior mid level",
                4 or 5 => "mid level",
                6 or 7 => "senior",
                >= 8 => "senior lead",
                _ => string.Empty
            };
        }

    }
}
