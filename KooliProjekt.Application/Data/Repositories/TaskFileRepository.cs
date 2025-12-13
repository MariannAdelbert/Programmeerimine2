using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public class TaskFileRepository : BaseRepository<TaskFile>, ITaskFileRepository
    {
        public TaskFileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
