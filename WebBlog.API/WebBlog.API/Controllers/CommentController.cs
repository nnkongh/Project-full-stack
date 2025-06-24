using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBlog.API.Mapper;
using WebBlog.API.Repository;
using WebBlog.API.ViewModel.Comment;

namespace WebBlog.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CommentController : ControllerBase
    {
        public readonly ICommentRepository _cmtRepo;
        public readonly IBlogRepository _blogRepo;

        public CommentController(ICommentRepository cmtRepo,IBlogRepository blogRepo) {
            _cmtRepo = cmtRepo;
            _blogRepo = blogRepo;
        }
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<IActionResult> GetByBlogId(int id)
        {
            var comment = await _cmtRepo.GetByBlogIdAsync(id);
            var result = comment.Select(c => c.MapComment()).ToList();
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("Create/{blogId:int}")]
        public async Task<IActionResult> Create(CreateCommentDTO item, int blogId)
        {
            if(!await _blogRepo.IfExists(blogId))
            {
                return NotFound();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var cmtDto = item.MapCreateComment(blogId, userId);
            await _cmtRepo.CreateAsync(cmtDto);
            return CreatedAtAction(nameof(GetByBlogId), new { id = cmtDto.Id }, cmtDto.MapComment());
        }
        [HttpPut]
        [Route("Update/{Id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCommentDTO item)
        {
            var comment = await _cmtRepo.UpdateAsync(item.MapUpdateComment(),id);
            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.MapComment());
        }
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete (int id)
        {
            var comment = await _cmtRepo.DeleteAsync(id);
            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.MapComment());
        }
    }
}
