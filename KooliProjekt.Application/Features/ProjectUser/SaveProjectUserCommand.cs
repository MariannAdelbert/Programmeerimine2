using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class SaveProjectUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string RoleInProject { get; set; }
    }
}