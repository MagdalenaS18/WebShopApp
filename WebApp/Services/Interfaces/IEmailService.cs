using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}
