using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TaskFiles
{
    [ExcludeFromCodeCoverage]
    public class GetTaskFileQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
