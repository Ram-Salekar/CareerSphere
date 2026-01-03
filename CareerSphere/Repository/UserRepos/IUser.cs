using CareerSphere.ApiModels.AuthModels;
using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.ApiModels.UsersApiModels;

namespace CareerSphere.Repository.UserRepoFolder
{
    public interface IUser
    {
        public Task<List<UserResponseApiModel>> GetUsersAsync ();
        public Task<UserResponseApiModel> GetUserByIdAsync (Guid id);
        public Task<string> GetUserName(Guid id);
        public Task<Boolean> CreateUserAsync(UserCreateApiModel userCreateApiModel);
        public Task<string ?> getToken (LoginApiModel detail);
        public Task<UserResponseApiModel?>GetUserByEmailOrUserName(string emailOrUsername);


    }
}
