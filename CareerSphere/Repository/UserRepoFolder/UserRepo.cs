using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.ApiModels.UsersApiModels;
using CareerSphere.Data;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Models.UserTableModel;
using Microsoft.EntityFrameworkCore;
namespace CareerSphere.Repository.UserRepoFolder
{
    public class UserRepo : IUser

    {
        private readonly AppDbContext _dbContext;

        public UserRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UserResponseApiModel>> GetUsersAsync()
        {
           
            var users = await _dbContext.Users
                                        .Include(u => u.Posts)
                                        .ToListAsync();

           
            var userResponseApiModels = users.Select(user => new UserResponseApiModel
            {
                id = user.id,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                profileImageUrl = user.profileImageUrl,
                about = user.about,
                header = user.header,
                username = user.username,
                createdBy = user.createdBy,
                    createdAt = user.createdAt,
                    modifiedBy = user.modifiedBy,
                    lastmodifiedAt = user.lastmodifiedAt,

                postResponses = user.Posts.Select(post => new PostResponseApiModel
                {
                    id = post.id,
                    UserId = post.UserId,
                    description = post.description,
                    contentImageUrl = post.contentImageUrl,
                    createdBy = post.createdBy,
                    createdAt = post.createdAt,
                    modifiedBy = post.modifiedBy,
                    lastmodifiedAt = post.lastmodifiedAt

                }).ToList()

            }).ToList();

            // 3️⃣ Return
            return userResponseApiModels;
        }


    }
    }

