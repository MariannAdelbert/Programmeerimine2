using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, OperationResult<PagedResult<UserDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<UserDto>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));
            var result = new OperationResult<PagedResult<UserDto>>();

            if (request.Page <= 0 || request.PageSize <= 0) return result;

            var query = _dbContext.Users.OrderBy(u => u.Id).Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            });

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            result.Value = new PagedResult<UserDto>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = total,
                Results = items
            };

            return result;
        }
    }
}