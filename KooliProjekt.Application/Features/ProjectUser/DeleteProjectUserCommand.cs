using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    [ExcludeFromCodeCoverage]
    public class DeleteProjectUserCommand : IRequest<OperationResult>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
