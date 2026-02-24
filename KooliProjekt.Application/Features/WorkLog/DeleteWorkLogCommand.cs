using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.WorkLogs
{
    [ExcludeFromCodeCoverage]
    public class DeleteWorkLogCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
