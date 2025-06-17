using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Helper;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.Models.Pagination;
using WebBlog.API.Repository;
using WebBlog.API.ViewModel;

namespace WebBlog.API.Repo
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _db;  

        public BlogRepository(ApplicationDbContext db, IPhotoService photoService) {
            _db = db;
            _photoService = photoService;
        }
        public async Task<Blog> CreateAsync(Blog blog)
        {
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return blog;
        }

        public async Task<Blog?> DeleteAsync(int id)
        {
            var item = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            _db.Blogs.Remove(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<Pagination<Blog>> GetBlogs(QueryObject query)
        { 
            var item = _db.Blogs.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrEmpty(query.Tags))
            {
                item = item.Where(x => x.Tags.ToLower().Contains(query.Tags.ToLower()));
            }
            var paged = await Pagination<Blog>.CreateAsync(item, query.PageIndex, query.PageSize);
            return paged;
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            var item = await _db.Blogs.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            return item;
        }

        public async Task<bool> IfExists(int id)
        {
            return await _db.Blogs.AnyAsync(x => x.Id == id);
        }

        public async Task<Blog?> UpdateAsync(int id, BlogViewModel blog)
        {
            string imageUrl = string.Empty;
            if (blog.Image != null)
            {
                var photo = await _photoService.AddPhotoAsync(blog.Image);
                imageUrl = photo.Url.ToString();
            }

            var item = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return null;
            item.Title = blog.Title;
            item.Content = blog.Content;
            item.Tags = blog.Tags;
            item.Image = imageUrl;
            item.CreatedAt = blog.CreatedAt;
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
