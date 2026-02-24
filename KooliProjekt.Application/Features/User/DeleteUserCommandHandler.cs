using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            if (request.Id > 0)
            {
                var user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return result;
        }
    }
}