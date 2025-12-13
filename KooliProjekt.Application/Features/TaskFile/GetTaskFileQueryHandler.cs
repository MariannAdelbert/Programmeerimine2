using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class GetTaskFileQueryHandler : IRequestHandler<GetTaskFileQuery, OperationResult<object>>
    {
        private readonly ITaskFileRepository _repository;

        public GetTaskFileQueryHandler(ITaskFileRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<object>> Handle(GetTaskFileQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.AddError("TaskFile ei leitud.");
                return result;
            }

            result.Value = new { entity.Id };
            return result;
        }
    }
}
