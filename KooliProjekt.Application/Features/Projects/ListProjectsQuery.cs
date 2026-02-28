using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    [ExcludeFromCodeCoverage]
    public class ListProjectsQuery : IRequest<OperationResult<PagedResult<Project>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Title { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
