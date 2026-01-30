using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class GetWorkLogQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
