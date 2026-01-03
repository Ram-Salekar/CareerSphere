using AutoMapper;
using CareerSphere.ApiModels.AuthModels;
using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.ApiModels.UsersApiModels;
using CareerSphere.Data;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Models.UserTableModel;
using CareerSphere.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CareerSphere.Repository.UserRepoFolder
{
    public class UserRepo : IUser

    {
        private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserRepo(AppDbContext dbContext , ITokenService tokenService , IMapper mapper)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _mapper = mapper;
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

        public async Task<UserResponseApiModel> GetUserByIdAsync(Guid id)
        {
            var users = await _dbContext.Users
                                       .Include(u => u.Posts)
                                       .FirstOrDefaultAsync(u => u.id == id);
            if (users == null)
            {
                return null; 
            }
            
            var userResponseApiModels = new UserResponseApiModel
            {
                id = users.id,
                firstName = users.firstName,
                lastName = users.lastName,
                email = users.email,
                profileImageUrl = users.profileImageUrl,
                about = users.about,
                header = users.header,
                username = users.username,
                createdBy = users.createdBy,
                createdAt = users.createdAt,
                modifiedBy = users.modifiedBy,
                lastmodifiedAt = users.lastmodifiedAt,

                postResponses = users.Posts.Select(post => new PostResponseApiModel
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

            };

            return userResponseApiModels;


        }

        public async Task<string> GetUserName(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.username;
        }

        public async Task<bool> CreateUserAsync(UserCreateApiModel userCreateApiModel)
        {
            var hasher = new PasswordHasher<User>();
            User user = new User()
            {
                id = Guid.NewGuid(),
                firstName = userCreateApiModel.firstName,
                lastName = userCreateApiModel.lastName,
                dateOfBirth = userCreateApiModel.dateOfBirth,
                profileImageUrl = userCreateApiModel.profileImageUrl,
                about = userCreateApiModel.about,
                header = userCreateApiModel.header,
                username = userCreateApiModel.username,
                email = userCreateApiModel.email,
                passwordHash = userCreateApiModel.password,
                createdBy = userCreateApiModel.username,
                createdAt = DateTime.Now,
                modifiedBy = userCreateApiModel.username,
                lastmodifiedAt = DateTime.Now,
                Posts = new List<Post>()
            };
            user.passwordHash = hasher.HashPassword(user, user.passwordHash);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
        public async Task<string ?> getToken(LoginApiModel detail)
        {
            var user = await _dbContext.Users
                                        .FirstOrDefaultAsync(u => u.email == detail.emailOrUsername ||
                                        u.username == detail.emailOrUsername);
            if (user == null)
            {
                return null;
            }
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.passwordHash, detail.password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
           return  _tokenService.CreateToken(user);
        }

        public async Task<UserResponseApiModel?> GetUserByEmailOrUserName(string emailOrUsername)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.email == emailOrUsername ||
                                        u.username == emailOrUsername);
            if (user == null)
            {
                return null;

            }
            return _mapper.Map<UserResponseApiModel>(user);

        } 
    }
    }

