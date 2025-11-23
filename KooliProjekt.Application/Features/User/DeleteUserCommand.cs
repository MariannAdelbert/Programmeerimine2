using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
