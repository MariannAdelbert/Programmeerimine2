using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask> GetByIdAsync(int id);
        Task SaveAsync(ProjectTask entity);
    }
}
