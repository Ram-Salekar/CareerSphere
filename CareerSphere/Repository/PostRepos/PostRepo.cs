using AutoMapper;
using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Data;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Repository.UserRepoFolder;
using Microsoft.EntityFrameworkCore;

namespace CareerSphere.Repository.PostRepos
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly IUser _user;
        private readonly IMapper _mapper;

        public PostRepo(AppDbContext dbContext, IUser user, IMapper mapper)
        {
            _dbContext = dbContext;
            _user = user;
            _mapper = mapper;
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
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> Deletepost(Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<PostResponseApiModel>> PostByUserID(Guid id)
        {
            var posts = await _dbContext.Posts
                                       .FromSqlRaw(
                                       "SELECT * FROM Posts WHERE UserId = {0}",
                                       id
                                       )
                                       .AsNoTracking()
                                       .ToListAsync();
            return _mapper.Map<List<PostResponseApiModel>>(posts);

        }

        public async Task<List<PostResponseApiModel>> GetFeedPostsAsync(Guid userId)
        {
            var posts = await _dbContext.Posts
                .Where(p =>
                    _dbContext.Connections
                        .Where(c => c.followerId == userId)
                        .Select(c => c.followingId)
                        .Contains(p.UserId)
                        
                )
                .OrderByDescending(p => p.createdAt)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<PostResponseApiModel>>(posts);
        }
    }
}
