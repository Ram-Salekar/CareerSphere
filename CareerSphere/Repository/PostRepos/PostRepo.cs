using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Data;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Repository.UserRepoFolder;

namespace CareerSphere.Repository.PostRepos
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly IUser _user;

        public PostRepo(AppDbContext dbContext,  IUser user)
        {
            _dbContext = dbContext;
            _user = user;
            
        }
        public async Task<List<PostResponseApiModel>> GetPostsAsync()
        {
            List<Post> posts = _dbContext.Posts.ToList();
            List<PostResponseApiModel> postResponseApiModels = new List<PostResponseApiModel>();
            foreach (Post post in posts)
            {
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

        public async Task<Boolean> CreatePostAsync(PostCreateApiModel postCreateApiModel)
        {
            string username = _user.GetUserName(postCreateApiModel.UserId).Result;
            Post post = new Post()
            {
                UserId = postCreateApiModel.UserId,
                description = postCreateApiModel.description,
                contentImageUrl = postCreateApiModel.contentImageUrl,
                createdBy = username,
                createdAt = DateTime.Now,
                modifiedBy = username,
                lastmodifiedAt = DateTime.Now
            };
            _dbContext.Posts.Add(post);
            await  _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
