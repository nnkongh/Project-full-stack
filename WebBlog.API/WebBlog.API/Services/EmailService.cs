using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using WebBlog.API.Interface;
using WebBlog.API.Models;

namespace WebBlog.API.Services
{
    public class EmailService : IEmailService
    {
        public readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var email = CreateEmailMessage(message);
            Send(email);
        }

        private void Send(MimeMessage email)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port,true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(email);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    throw new Exception("Failed to send email", ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
                ;
            }
            ;
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email",_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;

        }
    }
}
