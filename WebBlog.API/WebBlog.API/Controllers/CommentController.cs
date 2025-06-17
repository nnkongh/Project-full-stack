using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Mapper;
using WebBlog.API.Repository;
using WebBlog.API.ViewModel.Comment;

namespace WebBlog.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        public readonly ICommentRepository _cmtRepo;
        public readonly IBlogRepository _blogRepo;

        public CommentController(ICommentRepository cmtRepo,IBlogRepository blogRepo) {
            _cmtRepo = cmtRepo;
            _blogRepo = blogRepo;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _cmtRepo.GetAllAsync();
            var commentDTOs = comments.Select(c => c.MapComment());
            return Ok(commentDTOs);
        }
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _cmtRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.MapComment());
        }
        [HttpPost]
        [Route("Create/{blogId:int}")]
        public async Task<IActionResult> Create(CreateCommentDTO item, int blogId)
        {
            if(!await _blogRepo.IfExists(blogId))
            {
                return NotFound();
            }
            var cmtDto = item.MapCreateComment(blogId);
            await _cmtRepo.CreateAsync(cmtDto);
            return CreatedAtAction(nameof(GetById), new { id = cmtDto.Id }, cmtDto.MapComment());
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
