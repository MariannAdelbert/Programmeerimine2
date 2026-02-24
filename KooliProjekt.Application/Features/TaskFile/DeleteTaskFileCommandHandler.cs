using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class DeleteTaskFileCommandHandler : IRequestHandler<DeleteTaskFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTaskFileCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteTaskFileCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            // Lae objekt InMemory-st
            var entity = await _dbContext.TaskFiles
                .FirstOrDefaultAsync(tf => tf.Id == request.Id, cancellationToken);

            if (entity != null)
            {
                _dbContext.TaskFiles.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}