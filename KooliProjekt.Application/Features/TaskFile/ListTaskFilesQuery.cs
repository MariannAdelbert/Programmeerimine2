using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class ListTaskFilesQuery : IRequest<OperationResult<PagedResult<TaskFileDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}