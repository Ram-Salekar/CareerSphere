using AutoMapper;
using CareerSphere.ApiModels.UsersApiModels;
using CareerSphere.Models.UserTableModel;

namespace CareerSphere.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User,UserResponseApiModel>().ReverseMap().ForMember(dest => dest.passwordHash , opt => opt.Ignore());
        }
    }
}
