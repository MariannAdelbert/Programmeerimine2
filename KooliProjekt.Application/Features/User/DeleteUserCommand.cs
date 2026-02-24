using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class DeleteUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
