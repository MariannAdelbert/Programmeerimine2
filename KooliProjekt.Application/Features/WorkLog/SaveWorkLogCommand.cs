using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class SaveWorkLogCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
