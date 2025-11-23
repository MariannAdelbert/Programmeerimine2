using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectCommandHandler : IRequestHandler<SaveProjectCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProjectCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Project project;

            if (request.Id == 0)
            {
                // Uus projekt
                project = new Project();
                await _dbContext.Projects.AddAsync(project, cancellationToken);
            }
            else
            {
                // Olemasolev projekt
                project = await _dbContext.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
                if (project == null)
                {
                    result.AddError("Projekt ei leitud.");
                    return result;
                }
            }

            // Väärtuste määramine
            project.Name = request.Name;
            project.StartDate = request.StartDate;
            project.Deadline = request.Deadline;
            project.Budget = request.Budget;
            project.HourlyRate = request.HourlyRate;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
