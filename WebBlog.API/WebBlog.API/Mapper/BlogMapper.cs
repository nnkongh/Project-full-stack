using WebBlog.API.Models;
using WebBlog.API.ViewModel.Blog;

namespace WebBlog.API.Mapper
{
    public static class BlogMapper
    {
        public static BlogDTO MapBlog(this Blog blog)
        {
            return new BlogDTO
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                Image = blog.Image,
                CreatedAt = blog.CreatedAt,
                Comments = blog.Comments.Select(c => c.MapComment()).ToList()
            };
        }
    }
}
