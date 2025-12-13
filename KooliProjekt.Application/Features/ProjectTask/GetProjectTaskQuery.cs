using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class GetProjectTaskQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
