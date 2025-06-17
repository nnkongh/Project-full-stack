using WebBlog.API.Models;

namespace WebBlog.API.ViewModel.Comment
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? BlogId { get; set; }
    }
}
