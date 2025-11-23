using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Kustuta esmalt seotud ProjectTasks
            await _dbContext.ProjectTasks
                .Where(pt => pt.ProjectId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Kustuta Project
            await _dbContext.Projects
                .Where(p => p.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
