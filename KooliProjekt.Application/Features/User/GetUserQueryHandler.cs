using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<object>>
    {
        private readonly IUserRepository _repository;

        public GetUserQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<object>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.AddError("User ei leitud.");
                return result;
            }

            result.Value = new { entity.Id };
            return result;
        }
    }
}
