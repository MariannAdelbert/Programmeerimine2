using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    [ExcludeFromCodeCoverage]
    public class SaveProjectUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string RoleInProject { get; set; }
    }
}