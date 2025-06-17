using System.Runtime.CompilerServices;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Comment;

namespace WebBlog.API.Mapper
{
    public static class CommentMapper
    {
        public static CommentDTO MapComment(this Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                BlogId = comment.BlogId,
                
            };
        }
        public static Comment MapCreateComment(this CreateCommentDTO cmtDTO, int blogId)
        {
            return new Comment
            {
                Content = cmtDTO.Content,
                BlogId = blogId,
            };
        }
        public static Comment MapUpdateComment(this UpdateCommentDTO cmtDTO)
        {
            return new Comment
            {
                Content = cmtDTO.Content,
            };
        }
    }
}
