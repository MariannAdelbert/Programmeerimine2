using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Projects
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, OperationResult<object>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<OperationResult<object>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var project = await _projectRepository.GetByIdAsync(request.Id);

            if (project == null)
            {
                result.AddError("Projekt ei leitud.");
                return result;
            }

            result.Value = new
            {
                Id = project.Id,
                Name = project.Name,
                StartDate = project.StartDate,
                Deadline = project.Deadline,
                Budget = project.Budget,
                HourlyRate = project.HourlyRate
            };

            return result;
        }

    }
}
