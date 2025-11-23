using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class DeleteWorkLogCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
