using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ProjectTaskRepository : BaseRepository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<ProjectTask> GetByIdAsync(int id)
        {
            return await DbContext.ProjectTasks
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task SaveAsync(ProjectTask entity)
        {
            if (entity.Id == 0)
                await DbContext.ProjectTasks.AddAsync(entity);
            else
                DbContext.ProjectTasks.Update(entity);

            await DbContext.SaveChangesAsync();
        }
    }
}
