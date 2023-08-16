using WebApp.Infrastructure;

namespace WebApp.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task Save();
    }
}
