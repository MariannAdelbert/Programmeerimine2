using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public abstract class BaseRepository <T> where T : Entity
    {
        protected ApplicationDbContext DbContext { get; private set; }

        protected BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync(T entity)
        {
            if (entity.Id == 0)
            {
                DbContext.Set<T>().Add(entity);
            }
            else
            {
                DbContext.Set<T>().Update(entity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                DbContext.Set<T>().Remove(entity);
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
