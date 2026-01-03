using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Models.PostTableModel;

namespace CareerSphere.Repository.PostRepos
{
    public interface IPostRepo
    {
        public Task<List<PostResponseApiModel>> GetPostsAsync();
        public Task<Boolean> CreatePostAsync(PostCreateApiModel postCreateApiModel);
    }
}
