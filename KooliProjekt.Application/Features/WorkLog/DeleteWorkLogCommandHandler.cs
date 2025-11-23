using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class DeleteWorkLogCommandHandler : IRequestHandler<DeleteWorkLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteWorkLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteWorkLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.WorkLogs
                .Where(wl => wl.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
