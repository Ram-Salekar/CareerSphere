using Microsoft.AspNetCore.Mvc;
using CareerSphere.ApiModels.PostApiModels;
using CareerSphere.Repository.PostRepos;
using Microsoft.AspNetCore.Authorization;

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

    }
}
