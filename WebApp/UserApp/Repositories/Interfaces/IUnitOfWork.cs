namespace UserApp.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task Save();
    }
}
