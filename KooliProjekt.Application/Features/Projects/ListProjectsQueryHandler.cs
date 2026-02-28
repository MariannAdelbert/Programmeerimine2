using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, OperationResult<PagedResult<Project>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<Project>>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<Project>>();

            var query = _dbContext.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(list => list.Name.Contains(request.Title));
            }

            if (request.IsCompleted.HasValue)
            {
                if (request.IsCompleted.Value)
                {
                    query = query.Where(list => list.ProjectTasks.All(item => item.IsCompleted));
                }
                else
                {
                    query = query.Where(list => list.ProjectTasks.Any(item => !item.IsCompleted));
                }
            }

            result.Value = await query
                .OrderBy(list => list.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;

            // Kui page või pageSize on <= 0, tagastame null Value
            /*if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            // Võtame andmed leheküljiti
            var query = _dbContext.Projects.OrderBy(p => p.Name);

            // InMemory DB jaoks teeb ToListAsync enne PagedResult-i loomist
            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);

            result.Value = pagedResult;
            return result;*/
        }
    }
}