using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class GetProjectUserQuery
    : IRequest<OperationResult<ProjectUserDto>>
    {
        public int Id { get; set; }
    }
}
