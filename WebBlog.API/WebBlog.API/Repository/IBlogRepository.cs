using WebBlog.API.Helper;
using WebBlog.API.Models;
using WebBlog.API.Models.Pagination;
using WebBlog.API.ViewModel;

namespace WebBlog.API.Repository
{
    public interface IBlogRepository
    {
        Task<Blog?> GetByIdAsync(int id);
        Task<Blog> CreateAsync(Blog blog);
        Task<Blog?> DeleteAsync(int id);
        Task<Blog?> UpdateAsync(int id, BlogViewModel blog);
        Task<Pagination<Blog>> GetBlogs(QueryObject querry);
        Task<bool> IfExists(int id);

    }
}