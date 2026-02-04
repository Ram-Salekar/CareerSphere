using Microsoft.AspNetCore.Mvc;
using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Repository.PostRepos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CareerSphere.Controllers
{
    [Authorize]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepo _postRepo;

        public PostController(IPostRepo postRepo)
        {
            _postRepo = postRepo;
        }


        [HttpGet("api/posts")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var posts = await _postRepo.GetPostsAsync();
            return Ok(posts);
        }

        [HttpPost("api/posts")]
        public async Task<IActionResult> CreatePostAsync([FromBody] PostCreateApiModel postCreateApiModel)
        {
            var post = await _postRepo.CreatePostAsync(postCreateApiModel);
            return Ok(post);
        }
        [HttpGet("api/postById")]
        public async Task<IActionResult> PostByUserID()
        {
            Guid id = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
            var posts = await _postRepo.PostByUserID(id);
            return Ok(posts);
        }
        [HttpDelete("api/posts/{id}")]
        public async Task<IActionResult> Deletepost(Guid id)
        {
            var result = await _postRepo.Deletepost(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("api/feed")]
        public async Task<IActionResult> GetPostByIdAsync() {
            Guid id = User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            var posts = await _postRepo.GetFeedPostsAsync(id);
            return Ok(posts);
        }





    }
}
