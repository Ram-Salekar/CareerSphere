using AutoMapper;
using CareerSphere.ApiModels.ExperienceApiModel;
using CareerSphere.Controllers;
using CareerSphere.Models.EducationTableModel;

namespace CareerSphere.Mapper
{
    public class ExperienceMapper : Profile
    {
        public ExperienceMapper()
        {
            CreateMap<ExperiencePostApimodel, Experience>();
            CreateMap<Experience, ExperienceRepsoneApiModel>();
        }
    }
}
