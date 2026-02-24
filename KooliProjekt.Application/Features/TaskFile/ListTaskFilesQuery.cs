using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TaskFiles
{
    [ExcludeFromCodeCoverage]
    public class ListTaskFilesQuery : IRequest<OperationResult<PagedResult<TaskFileDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}