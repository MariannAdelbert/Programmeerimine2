using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class GetProjectTaskQueryHandler : IRequestHandler<GetProjectTaskQuery, OperationResult<object>>
    {
        private readonly IProjectTaskRepository _repository;

        public GetProjectTaskQueryHandler(IProjectTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<object>> Handle(GetProjectTaskQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>(); // ← Siin defineeritakse result

            var task = await _repository.GetByIdAsync(request.Id);

            if (task == null)
            {
                result.AddError("ProjectTask ei leitud.");
                return result;
            }

            result.Value = new
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title
            };

            return result;
        }
    }
}
