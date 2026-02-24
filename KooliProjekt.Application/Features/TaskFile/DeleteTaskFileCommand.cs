using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TaskFiles
{
    [ExcludeFromCodeCoverage]
    public class DeleteTaskFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
