using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.WorkLogs
{
    [ExcludeFromCodeCoverage]
    public class GetWorkLogQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
