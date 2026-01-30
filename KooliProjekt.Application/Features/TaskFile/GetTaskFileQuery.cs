using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class GetTaskFileQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
