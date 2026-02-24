using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    [ExcludeFromCodeCoverage]
    public class GetProjectUserQuery
    : IRequest<OperationResult<ProjectUserDto>>
    {
        public int Id { get; set; }
    }
}
