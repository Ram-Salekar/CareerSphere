using Microsoft.AspNetCore.Mvc;
using CareerSphere.Repository;

namespace CareerSphere.Controllers
{
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

    }
}
