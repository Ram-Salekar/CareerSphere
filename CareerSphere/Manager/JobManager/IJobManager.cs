using CareerSphere.ApiModels.JSearchApiModels;

namespace CareerSphere.Manager.JobManager
{
    public interface IJobManager
    {
        Task<List<JobListing>> GetJobsForResumeAsync( string role, 
            string location,
            List<string> skills, int exp );
    }
}
