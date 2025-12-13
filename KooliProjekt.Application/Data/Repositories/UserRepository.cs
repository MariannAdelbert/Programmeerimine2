using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveAsync(User user)
        {
            if (user.Id == 0)
            {
                await DbContext.Users.AddAsync(user);
            }
            else
            {
                DbContext.Users.Update(user);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
