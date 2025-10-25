using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Projects
{
    public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, OperationResult<IList<Project>>>
    {

        private readonly ApplicationDbContext _dbContext;
        public ListProjectsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OperationResult<IList<Project>>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Project>>();
            result.Value = await _dbContext
                .Projects
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}
