using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class GetProjectTaskQueryHandler : IRequestHandler<GetProjectTaskQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProjectTaskQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetProjectTaskQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            if (request == null)
                return result;

            result.Value = await _dbContext.ProjectTasks
                .Where(pt => pt.Id == request.Id)
                .Select(pt => new
                {
                    pt.Id,
                    pt.ProjectId,
                    pt.Title,
                    pt.Description,
                    pt.StartDate,
                    pt.EstimatedHours,
                    pt.IsCompleted,
                    pt.FixedPrice,
                    pt.ResponsibleUserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
