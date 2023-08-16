using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(Email email)
        {
            var message = new MimeMessage();
            var address = _configuration.GetSection("EmailConfiguration")["From"];

            message.From.Add(new MailboxAddress("Admin", address));
            message.To.AddRange(email.To);
            message.Subject = email.Subject;
            message.Body = new TextPart(TextFormat.Text)
            {
                Text = email.Body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    var smtp = _configuration.GetSection("EmailConfiguration")["SmtpServer"];
                    var port = _configuration.GetSection("EmailConfiguration")["Port"];
                    var username = _configuration.GetSection("EmailConfiguration")["Username"];
                    var password = _configuration.GetSection("EmailConfiguration")["Password"];

                    await client.ConnectAsync(smtp, int.Parse(port), SecureSocketOptions.Auto);
                    
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(username, password);

                    await client.SendAsync(message);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
