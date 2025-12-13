using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly IUserRepository _repository;

        public SaveUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            User entity;

            if (request.Id != 0)
            {
                entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    result.AddError("User ei leitud.");
                    return result;
                }
            }
            else
            {
                entity = new User();
            }

            await _repository.SaveAsync(entity);
            return result;
        }
    }
}
