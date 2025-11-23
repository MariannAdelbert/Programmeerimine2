using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class DeleteProjectUserCommandHandler : IRequestHandler<DeleteProjectUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProjectUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.ProjectUsers
                .Where(pu => pu.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
