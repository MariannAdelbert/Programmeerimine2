using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    [ExcludeFromCodeCoverage]
    public class GetProjectTaskQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
