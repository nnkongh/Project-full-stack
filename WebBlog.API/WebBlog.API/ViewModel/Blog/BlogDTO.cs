using System.ComponentModel.DataAnnotations;
using WebBlog.API.ViewModel.Comment;

namespace WebBlog.API.ViewModel.Blog
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CommentDTO> Comments { get; set; } = [];
    }
}
