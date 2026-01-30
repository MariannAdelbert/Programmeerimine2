using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class GetWorkLogQueryHandler : IRequestHandler<GetWorkLogQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetWorkLogQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetWorkLogQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            if (request == null)
                return result;

            result.Value = await _dbContext.WorkLogs
                .Where(wl => wl.Id == request.Id)
                .Select(wl => new
                {
                    wl.Id,
                    wl.TaskId,
                    wl.UserId,
                    wl.Date,
                    wl.HoursSpent,
                    wl.Description
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
