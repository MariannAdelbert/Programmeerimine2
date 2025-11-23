using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class DeleteProjectTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectTaskCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.ProjectTasks
                .Where(pt => pt.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
