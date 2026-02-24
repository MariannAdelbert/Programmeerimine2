using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class SaveTaskFileCommandHandler : IRequestHandler<SaveTaskFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTaskFileCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(SaveTaskFileCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            var result = new OperationResult();

            TaskFile entity;

            if (request.Id > 0)
            {
                entity = await _dbContext.TaskFiles.FindAsync(request.Id);
                if (entity == null)
                    return result;
            }
            else
            {
                entity = new TaskFile();
                await _dbContext.TaskFiles.AddAsync(entity, cancellationToken);
            }

            entity.TaskId = request.TaskId;
            entity.FileName = request.FileName;
            entity.FilePath = request.FilePath;
            entity.UploadDate = request.UploadDate;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}