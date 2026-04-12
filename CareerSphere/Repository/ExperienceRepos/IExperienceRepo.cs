using CareerSphere.ApiModels.ExperienceApiModel;

namespace CareerSphere.Repository.ExperienceRepos
{
    public interface IExperienceRepo
    {
        public Task<bool> AddExperience(ExperiencePostApimodel experience,Guid userId);
        public Task<bool> UpdateExperience(ExperiencePostApimodel experience ,Guid userId);
        public Task<ExperienceRepsoneApiModel> GetExperienceById(Guid experienceId);
        public Task<List<ExperienceRepsoneApiModel>> GetAllExperienceByUserId(Guid userId);
        public Task<bool> DeleteExperience(Guid experienceId);
    }
}
