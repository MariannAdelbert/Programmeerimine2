using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            // Kui Id <= 0, ei tee midagi (tagastame lihtsalt tühja OperationResult)
            if (request.Id <= 0)
                return result;

            // Leia task
            var task = await _dbContext.ProjectTasks
                .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);

            if (task != null)
            {
                _dbContext.ProjectTasks.Remove(task);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}