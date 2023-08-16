using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure;
using WebApp.Models;
using WebApp.Repositories.Interfaces;

namespace WebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public Task ChangePassword(long id, string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(long id)
        {
            _userDbContext.Users.Remove(_userDbContext.Users.Find(id));
        }

        public async Task<List<User>> GetAll()
        {
            return await _userDbContext.Users.ToListAsync();
        }

        public async Task<User> GetById(long id)
        {
            return await _userDbContext.Users.FindAsync(id);
        }

        public Task<byte[]> GetUserImage(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> InsertUser(User user)
        {
            await _userDbContext.Users.AddAsync(user);
            return user;
        }

        public void UpdateUser(User user)
        {
            User newUser = _userDbContext.Users.Find(user.Id);
            newUser = user;
            _userDbContext.Users.Update(newUser);
        }
    }
}
