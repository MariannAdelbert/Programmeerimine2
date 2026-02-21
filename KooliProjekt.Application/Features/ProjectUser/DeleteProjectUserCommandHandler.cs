using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class DeleteProjectUserCommandHandler : IRequestHandler<DeleteProjectUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProjectUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteProjectUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            var projectUser = await _dbContext.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == request.ProjectId && pu.UserId == request.UserId, cancellationToken);

            if (projectUser != null)
            {
                _dbContext.ProjectUsers.Remove(projectUser);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}