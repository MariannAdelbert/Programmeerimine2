using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class DeleteTaskFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
