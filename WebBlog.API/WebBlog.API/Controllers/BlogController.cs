using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Helper;
using WebBlog.API.Interface;
using WebBlog.API.Mapper;
using WebBlog.API.Models;
using WebBlog.API.Models.Pagination;
using WebBlog.API.Repository;
using WebBlog.API.ViewModel;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repo;
        private readonly ILogger<BlogController> _logger;
        private readonly IPhotoService _photo;

        public BlogController(IBlogRepository repo, IPhotoService photo, ILogger<BlogController> logger)
        {
            _repo = repo;
            _logger = logger;
            _photo = photo;
        }
        [HttpGet("blogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs([FromQuery] QueryObject query)
        {
            var items = await _repo.GetBlogs(query);
            return Ok(items);
        }
        [HttpGet("{id:int}")] // Route constraint
        // Custom Route Regex
        public async Task<ActionResult<Blog?>> GetBlog(int id)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog.MapBlog());
        }
        [HttpPost("create")]
        public async Task<ActionResult<Blog>> CreateBlog([FromForm]BlogViewModel blog)
        {
            string imageUrl = string.Empty;
            if (blog.Image != null)
            {
                var photo = await _photo.AddPhotoAsync(blog.Image);
                imageUrl = photo.Url.ToString();
            }
            var Blog = new Blog
            {
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                Image = imageUrl,
                CreatedAt = DateTime.Now,
            };
            
            var item = await _repo.CreateAsync(Blog);
            return CreatedAtAction(nameof(GetBlog), new { id = item.Id }, item);
        }
        [HttpPost("update/{id:int}")]
        public async Task<ActionResult<Blog?>> UpdateBlog(int id, [FromForm]BlogViewModel blog)
        {
         
            var item = await _repo.UpdateAsync(id, blog);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpDelete("delete/{id:int}")] // Route constraint    ??Custom Route Regex??
        public async Task<ActionResult<Blog?>> DeleteBlog(int id)
        {
            var item = await _repo.DeleteAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
