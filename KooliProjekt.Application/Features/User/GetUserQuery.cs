using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
