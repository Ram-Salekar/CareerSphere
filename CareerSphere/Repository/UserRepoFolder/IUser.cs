using CareerSphere.ApiModels.UsersApiModels;

namespace CareerSphere.Repository.UserRepoFolder
{
    public interface IUser
    {
        public Task<List<UserResponseApiModel>> GetUsersAsync ();
    }
}
