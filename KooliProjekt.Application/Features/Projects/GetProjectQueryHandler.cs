using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class GetProjectQueryHandler
    : IRequestHandler<GetProjectQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProjectQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(
            GetProjectQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            if (request == null)
                return result;

            result.Value = await _dbContext
                .Projects
                .Where(p => p.Id == request.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    StartDate = p.StartDate,
                    Deadline = p.Deadline,
                    Budget = p.Budget,
                    HourlyRate = p.HourlyRate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }

}
