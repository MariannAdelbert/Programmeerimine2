using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class ListTaskFilesQueryHandler : IRequestHandler<ListTaskFilesQuery, OperationResult<PagedResult<TaskFileDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTaskFilesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<TaskFileDto>>> Handle(ListTaskFilesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<TaskFileDto>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = null;
                return result;
            }

            var query = _dbContext.TaskFiles
                .OrderBy(tf => tf.Id)
                .Select(tf => new TaskFileDto
                {
                    Id = tf.Id,
                    TaskId = tf.TaskId,
                    FileName = tf.FileName,
                    FilePath = tf.FilePath,
                    UploadDate = tf.UploadDate
                });

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            result.Value = new PagedResult<TaskFileDto>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalCount = total,
                Results = items
            };

            return result;
        }
    }
}