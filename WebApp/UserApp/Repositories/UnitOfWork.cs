using UserApp.Infrastructure;
using UserApp.Repositories.Interfaces;

namespace UserApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext _userDbContext;

        public IUserRepository Users { get; }

        public UnitOfWork(UserDbContext userDbContext, IUserRepository userRepository)
        {
            _userDbContext = userDbContext;
            Users = userRepository;
        }

        public async Task Save()
        {
            await _userDbContext.SaveChangesAsync();
        }
    }
}
