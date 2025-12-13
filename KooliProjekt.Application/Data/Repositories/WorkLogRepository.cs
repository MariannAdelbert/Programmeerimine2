using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public class WorkLogRepository : BaseRepository<WorkLog>, IWorkLogRepository
    {
        public WorkLogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
