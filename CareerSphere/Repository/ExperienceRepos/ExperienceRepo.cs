using AutoMapper;
using CareerSphere.ApiModels.ExperienceApiModel;
using CareerSphere.Data;
using CareerSphere.Models.EducationTableModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CareerSphere.Repository.ExperienceRepos
{
    public class ExperienceRepo : IExperienceRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;

        public ExperienceRepo(AppDbContext context,IMapper mapper) {
              _context = context;
            this.mapper = mapper;   
        }

        public async Task<bool> AddExperience(ExperiencePostApimodel experience, Guid userId)
            {
             Experience experienceEntity = mapper.Map<Experience>(experience);
            experienceEntity.experienceId = Guid.NewGuid();
            experienceEntity.userId = userId;


            _context.Experiences.Add(experienceEntity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExperience(Guid experienceId)
        {

           var experience = _context.Experiences.FirstOrDefault(e => e.experienceId == experienceId);
            if (experience == null)
            {
                return false;
            }

            _context.Experiences.Remove(experience);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ExperienceRepsoneApiModel>> GetAllExperienceByUserId(Guid userId)
        {
            var experiences = await _context.Experiences.Where(e => e.userId == userId).ToListAsync();
            return mapper.Map<List<ExperienceRepsoneApiModel>>(experiences);
        }

        public async Task<ExperienceRepsoneApiModel> GetExperienceById(Guid experienceId)
        {
            var experience = await _context.Experiences.FirstOrDefaultAsync(e => e.experienceId == experienceId);
            return mapper.Map<ExperienceRepsoneApiModel>(experience);
        }
        public async Task<bool> UpdateExperience(ExperiencePostApimodel experience, Guid userId)
        {
            var experienceEntity = await _context.Experiences.FirstOrDefaultAsync(e => e.experienceId == experience.experienceId);
            if (experienceEntity == null)
            {
                return false;
            }

            mapper.Map(experience, experienceEntity);

            _context.Experiences.Update(experienceEntity);
            return await _context.SaveChangesAsync() > 0;

        }
    }
}
