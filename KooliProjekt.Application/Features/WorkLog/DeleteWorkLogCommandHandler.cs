using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class DeleteWorkLogCommandHandler : IRequestHandler<DeleteWorkLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteWorkLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteWorkLogCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            var result = new OperationResult();

            if (request.Id > 0)
            {
                var workLog = await _dbContext.WorkLogs.FindAsync(new object[] { request.Id }, cancellationToken);
                if (workLog != null)
                {
                    _dbContext.WorkLogs.Remove(workLog);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return result;
        }
    }
}