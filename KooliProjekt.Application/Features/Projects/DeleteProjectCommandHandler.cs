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
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            // Ei tee midagi, kui Id on 0 või negatiivne
            if (request.Id <= 0)
                return result;

            // Kustuta esmalt seotud ProjectTasks (client-side delete InMemory DB jaoks)
            var tasksToDelete = _dbContext.ProjectTasks
                .Where(pt => pt.ProjectId == request.Id)
                .ToList();

            if (tasksToDelete.Any())
                _dbContext.ProjectTasks.RemoveRange(tasksToDelete);

            // Kustuta Project
            var project = await _dbContext.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
            if (project != null)
                _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}