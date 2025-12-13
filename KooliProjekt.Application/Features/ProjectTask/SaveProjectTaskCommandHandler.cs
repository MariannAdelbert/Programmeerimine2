using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommandHandler : IRequestHandler<SaveProjectTaskCommand, OperationResult>
    {
        private readonly IProjectTaskRepository _repository;

        public SaveProjectTaskCommandHandler(IProjectTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult> Handle(SaveProjectTaskCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            ProjectTask entity;

            if (request.Id != 0)
            {
                entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    result.AddError("ProjectTask ei leitud."); // ✅ õige viis
                    return result;
                }
            }
            else
            {
                entity = new ProjectTask();
            }

            entity.ProjectId = request.ProjectId;
            entity.Title = request.Title;

            await _repository.SaveAsync(entity);

            return result;
        }
    }
}
