using CareerSphere.ApiModels.JSearchApiModels;
using CareerSphere.Manager.JserviceManager;

namespace CareerSphere.Manager.JobManager
{
    public class JobManager : IJobManager
    {
        private readonly IJservice jservice;

        public JobManager(IJservice jservice)
        {
            this.jservice = jservice;
        }

        public async Task<List<JobListing>> GetJobsForResumeAsync(
            string role,
            string location,
            List<string> skills,int exp)
        {
           
            return await jservice.FetchJobsAsync(role, location, skills,exp);
        }
    }
}
