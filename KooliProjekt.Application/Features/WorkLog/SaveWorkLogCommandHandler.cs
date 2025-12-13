using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class SaveWorkLogCommandHandler : IRequestHandler<SaveWorkLogCommand, OperationResult>
    {
        private readonly IWorkLogRepository _repository;

        public SaveWorkLogCommandHandler(IWorkLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult> Handle(SaveWorkLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            WorkLog entity;

            if (request.Id != 0)
            {
                entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    result.AddError("WorkLog ei leitud.");
                    return result;
                }
            }
            else
            {
                entity = new WorkLog();
            }

            await _repository.SaveAsync(entity);
            return result;
        }
    }
}
