using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class DeleteTaskFileCommandHandler : IRequestHandler<DeleteTaskFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTaskFileCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTaskFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.TaskFiles
                .Where(tf => tf.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
