using WebApp.Models;

namespace WebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetById(long id);
        Task<User> InsertUser(User user);
        void DeleteUser(long id);
        void UpdateUser(User user);
        Task<byte[]> GetUserImage(long id);
        Task ChangePassword(long id, string password);
    }
}
