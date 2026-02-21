using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class DeleteProjectUserCommand : IRequest<OperationResult>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
