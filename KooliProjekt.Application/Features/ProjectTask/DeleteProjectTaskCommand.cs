using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class DeleteProjectTaskCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
