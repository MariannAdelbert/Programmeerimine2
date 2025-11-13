using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class ListProjectsQuery : IRequest<OperationResult<PagedResult<Project>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
