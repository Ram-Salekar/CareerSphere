using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Models.PostTableModel;

namespace CareerSphere.Repository
{
    public interface IPostRepo
    {
        public Task<List<PostResponseApiModel>> GetPostsAsync ();
    }
}
