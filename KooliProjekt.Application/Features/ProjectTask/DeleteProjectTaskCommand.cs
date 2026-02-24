using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    [ExcludeFromCodeCoverage]
    public class DeleteProjectTaskCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
