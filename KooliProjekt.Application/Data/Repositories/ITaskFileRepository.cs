using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ITaskFileRepository
    {
        Task<TaskFile> GetByIdAsync(int id);
        Task SaveAsync(TaskFile entity);
    }
}
