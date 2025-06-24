using Microsoft.EntityFrameworkCore;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Models;

namespace WebBlog.API.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment cmt)
        {
            await _context.Comments.AddAsync(cmt);
            await _context.SaveChangesAsync();
            return cmt;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var item = await _context.Comments.FindAsync(id);
            if(item == null)
            {
                return null;
            }
            _context.Comments.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<List<Comment?>> GetByBlogIdAsync(int blogId)
        {
            var comment = await _context.Comments.Include(c => c.User).Where(c => c.BlogId == blogId).OrderByDescending(c => c.CreatedAt).ToListAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(Comment cmt, int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null)
            {
                return null;
            }
            comment.Content = cmt.Content;
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
