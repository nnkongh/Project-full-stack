using WebBlog.API.Models;

namespace WebBlog.API.Interface
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
