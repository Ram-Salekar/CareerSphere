using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Data;
using CareerSphere.Models.PostTableModel;

namespace CareerSphere.Repository
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _dbContext;

        public PostRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<PostResponseApiModel>> GetPostsAsync()
        {
            List<Post> posts = _dbContext.Posts.ToList();
            List<PostResponseApiModel> postResponseApiModels = new List<PostResponseApiModel>();
            foreach (Post post in posts) {
                PostResponseApiModel postResponseApiModel = new PostResponseApiModel()
                {
                    id = post.id,
                    UserId = post.UserId,
                    description = post.description,
                    contentImageUrl = post.contentImageUrl,
                    createdBy = post.createdBy,
                    createdAt = post.createdAt,
                    modifiedBy = post.modifiedBy,
                    lastmodifiedAt = post.lastmodifiedAt
                    
                };
                postResponseApiModels.Add(postResponseApiModel);
            }
            return postResponseApiModels;
        }

       
    }
}
