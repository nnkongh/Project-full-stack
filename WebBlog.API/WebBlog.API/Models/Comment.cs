namespace WebBlog.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
