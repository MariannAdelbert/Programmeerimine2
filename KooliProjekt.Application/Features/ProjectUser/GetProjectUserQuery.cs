using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class GetProjectUserQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
