using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectCommandHandler : IRequestHandler<SaveProjectCommand, OperationResult>
    {
        private readonly IProjectRepository _projectRepository;

        public SaveProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<OperationResult> Handle(SaveProjectCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Kui Id = 0, teeme uue; kui mitte, loeme olemasoleva
            var project = new Project();
            if (request.Id != 0)
            {
                project = await _projectRepository.GetByIdAsync(request.Id);

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

            // Salvestamine
            await _projectRepository.SaveAsync(project);

            return result;
        }
    }
}
