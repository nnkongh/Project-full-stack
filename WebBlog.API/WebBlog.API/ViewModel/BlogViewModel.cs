using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
