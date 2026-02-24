using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class SaveWorkLogCommandHandler : IRequestHandler<SaveWorkLogCommand, OperationResult<WorkLogDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveWorkLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<WorkLogDto>> Handle(SaveWorkLogCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));

            WorkLog entity;
            if (request.Id.HasValue)
            {
                entity = await _dbContext.WorkLogs.FindAsync(new object[] { request.Id.Value }, cancellationToken);
                if (entity == null) return new OperationResult<WorkLogDto>();
            }
            else
            {
                entity = new WorkLog();
                await _dbContext.WorkLogs.AddAsync(entity, cancellationToken);
            }

            entity.TaskId = request.TaskId;
            entity.UserId = request.UserId;
            entity.Date = request.Date;
            entity.HoursSpent = request.HoursSpent;
            entity.Description = request.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult<WorkLogDto>
            {
                Value = new WorkLogDto
                {
                    Id = entity.Id,
                    TaskId = entity.TaskId,
                    UserId = entity.UserId,
                    Date = entity.Date,
                    HoursSpent = entity.HoursSpent,
                    Description = entity.Description
                }
            };
        }
    }
}