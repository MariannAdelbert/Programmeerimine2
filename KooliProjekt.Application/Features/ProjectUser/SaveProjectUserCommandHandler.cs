using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class SaveProjectUserCommandHandler : IRequestHandler<SaveProjectUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProjectUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(SaveProjectUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            ProjectUser projectUser;

            if (request.Id == 0)
            {
                // Uus ProjectUser
                projectUser = new ProjectUser();
                await _dbContext.ProjectUsers.AddAsync(projectUser, cancellationToken);
            }
            else
            {
                // Olemasolev ProjectUser
                projectUser = await _dbContext.ProjectUsers.FindAsync(new object[] { request.Id }, cancellationToken);
                if (projectUser == null)
                {
                    result.AddError("ProjectUser ei leitud.");
                    return result;
                }
            }

            // Väärtuste määramine
            projectUser.ProjectId = request.ProjectId;
            projectUser.UserId = request.UserId;
            projectUser.RoleInProject = request.RoleInProject;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}