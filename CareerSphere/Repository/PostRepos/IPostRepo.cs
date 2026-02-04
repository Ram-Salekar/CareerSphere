using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Models.PostTableModel;

namespace CareerSphere.Repository.PostRepos
{
    public interface IPostRepo
    {
        public Task<List<PostResponseApiModel>> GetPostsAsync();
        public Task<Boolean> CreatePostAsync(PostCreateApiModel postCreateApiModel);
        public Task<Boolean> Deletepost(Guid id);
        public Task<List<PostResponseApiModel>> PostByUserID(Guid UserId);
        public Task<List<PostResponseApiModel>> GetFeedPostsAsync(Guid UserId);
    }
}
