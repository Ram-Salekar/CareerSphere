using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Models.PostTableModel;
using AutoMapper;
namespace CareerSphere.Mapper
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<PostCreateApiModel, Post>();
            CreateMap<Post, PostResponseApiModel>();
        }
    }
}
