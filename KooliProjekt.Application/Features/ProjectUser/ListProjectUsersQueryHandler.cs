using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class ListProjectUsersQueryHandler
        : IRequestHandler<ListProjectUsersQuery, OperationResult<PagedResult<ProjectUserDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListProjectUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<ProjectUserDto>>> Handle(
            ListProjectUsersQuery request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Page <= 0 || request.PageSize <= 0) return new OperationResult<PagedResult<ProjectUserDto>>();

            var query = _dbContext.ProjectUsers.AsNoTracking().AsQueryable();

            // Võid lisada filtreid, nt ProjectId või RoleInProject
            if (request.ProjectId > 0)
            {
                query = query.Where(pu => pu.ProjectId == request.ProjectId);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var results = await query
                .OrderBy(pu => pu.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(pu => new ProjectUserDto
                {
                    Id = pu.Id,
                    ProjectId = pu.ProjectId,
                    UserId = pu.UserId,
                    RoleInProject = pu.RoleInProject
                })
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<ProjectUserDto>
            {
                Results = results,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            var result = new OperationResult<PagedResult<ProjectUserDto>>()
            {
                Value = pagedResult
            };

            return result;
        }
    }
}