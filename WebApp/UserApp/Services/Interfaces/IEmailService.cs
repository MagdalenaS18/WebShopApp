using UserApp.Models;

namespace UserApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}
