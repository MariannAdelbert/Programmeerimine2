using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class ListProjectUsersQueryHandler : IRequestHandler<ListProjectUsersQuery, OperationResult<PagedResult<ProjectUser>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<ProjectUser>>> Handle(ListProjectUsersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<ProjectUser>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.ProjectUsers
                .Include(pu => pu.User)
                .Include(pu => pu.Project)
                .OrderBy(pu => pu.RoleInProject);

            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);
            result.Value = pagedResult;
            return result;
        }
    }
}