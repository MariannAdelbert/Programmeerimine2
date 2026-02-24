using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class GetTaskFileQueryHandler : IRequestHandler<GetTaskFileQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTaskFileQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<object>> Handle(GetTaskFileQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            if (request == null)
                return result;

            result.Value = await _dbContext.TaskFiles
                .Where(tf => tf.Id == request.Id)
                .Select(tf => new TaskFileDto
                {
                    Id = tf.Id,
                    TaskId = tf.TaskId,
                    FileName = tf.FileName,
                    FilePath = tf.FilePath,
                    UploadDate = tf.UploadDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
