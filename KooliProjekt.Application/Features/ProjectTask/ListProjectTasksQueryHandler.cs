using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class ListProjectTasksQueryHandler : IRequestHandler<ListProjectTasksQuery, OperationResult<PagedResult<ProjectTask>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectTasksQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<ProjectTask>>> Handle(ListProjectTasksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<ProjectTask>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.ProjectTasks
                                  .Where(pt => pt.ProjectId == request.ProjectId)
                                  .OrderBy(pt => pt.Title);

            // InMemory DB jaoks ToListAsync enne PagedResult-i
            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);

            result.Value = pagedResult;
            return result;
        }
    }
}