using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    [ExcludeFromCodeCoverage]
    public class ListProjectTasksQuery : IRequest<OperationResult<PagedResult<ProjectTask>>>
    {
        public int ProjectId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}