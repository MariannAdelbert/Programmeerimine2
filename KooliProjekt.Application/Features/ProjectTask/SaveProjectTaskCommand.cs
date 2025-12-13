using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
    }
}
