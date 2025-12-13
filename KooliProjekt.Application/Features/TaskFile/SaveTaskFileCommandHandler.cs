using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class SaveTaskFileCommandHandler : IRequestHandler<SaveTaskFileCommand, OperationResult>
    {
        private readonly ITaskFileRepository _repository;

        public SaveTaskFileCommandHandler(ITaskFileRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult> Handle(SaveTaskFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            TaskFile entity;

            if (request.Id != 0)
            {
                entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    result.AddError("TaskFile ei leitud.");
                    return result;
                }
            }
            else
            {
                entity = new TaskFile();
            }

            await _repository.SaveAsync(entity);
            return result;
        }
    }
}
