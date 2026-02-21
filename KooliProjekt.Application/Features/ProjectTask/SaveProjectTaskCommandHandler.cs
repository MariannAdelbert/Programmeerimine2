using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommandHandler : IRequestHandler<SaveProjectTaskCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectTaskCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(SaveProjectTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();

            ProjectTask task;

            if (request.Id == 0)
            {
                task = new ProjectTask();
                await _dbContext.ProjectTasks.AddAsync(task, cancellationToken);
            }
            else
            {
                task = await _dbContext.ProjectTasks.FindAsync(new object[] { request.Id }, cancellationToken);
                if (task == null)
                {
                    result.AddError("ProjectTask ei leitud.");
                    return result;
                }
            }

            task.ProjectId = request.ProjectId;
            task.Title = request.Title;
            task.StartDate = request.StartDate;
            task.EstimatedHours = request.EstimatedHours;
            task.Description = request.Description;
            task.IsCompleted = request.IsCompleted;
            task.FixedPrice = request.FixedPrice;
            task.ResponsibleUserId = request.ResponsibleUserId;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}