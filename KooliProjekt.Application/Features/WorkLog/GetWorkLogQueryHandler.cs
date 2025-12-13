using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class GetWorkLogQueryHandler : IRequestHandler<GetWorkLogQuery, OperationResult<object>>
    {
        private readonly IWorkLogRepository _repository;

        public GetWorkLogQueryHandler(IWorkLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<object>> Handle(GetWorkLogQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.AddError("WorkLog ei leitud.");
                return result;
            }

            result.Value = new { entity.Id };
            return result;
        }
    }
}
