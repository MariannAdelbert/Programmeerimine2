using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class ListWorkLogsQueryHandler : IRequestHandler<ListWorkLogsQuery, OperationResult<PagedResult<WorkLogDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListWorkLogsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<WorkLogDto>>> Handle(ListWorkLogsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));
            var result = new OperationResult<PagedResult<WorkLogDto>>();

            if (request.Page <= 0 || request.PageSize <= 0) return result;

            var query = _dbContext.WorkLogs.OrderBy(w => w.Id).Select(w => new WorkLogDto
            {
                Id = w.Id,
                TaskId = w.TaskId,
                UserId = w.UserId,
                Date = w.Date,
                HoursSpent = w.HoursSpent,
                Description = w.Description
            });

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            result.Value = new PagedResult<WorkLogDto>
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