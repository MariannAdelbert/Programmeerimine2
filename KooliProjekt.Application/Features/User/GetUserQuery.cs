using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class GetUserQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
