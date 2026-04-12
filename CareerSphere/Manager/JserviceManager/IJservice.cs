using CareerSphere.ApiModels.JSearchApiModels;

namespace CareerSphere.Manager.JserviceManager
{
    public interface IJservice
    {
        Task<List<JobListing>> FetchJobsAsync(string role, string location, List<string> skills , int exps);
    }
}
