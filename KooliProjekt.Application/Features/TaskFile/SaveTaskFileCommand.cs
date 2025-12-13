using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class SaveTaskFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
