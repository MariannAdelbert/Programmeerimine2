using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class GetProjectUserQueryHandler
        : IRequestHandler<GetProjectUserQuery, OperationResult<ProjectUserDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProjectUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<ProjectUserDto>> Handle(
    GetProjectUserQuery request,
    CancellationToken cancellationToken)
        {
            var result = new OperationResult<ProjectUserDto>();

            // Null-turvaline: tagastame lihtsalt tühja OperationResult
            if (request == null)
                return result;

            if (request.Id <= 0)
                return result;

            result.Value = await _dbContext.ProjectUsers
                .Where(pu => pu.Id == request.Id)
                .Select(pu => new ProjectUserDto
                {
                    Id = pu.Id,
                    ProjectId = pu.ProjectId,
                    UserId = pu.UserId,
                    RoleInProject = pu.RoleInProject
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}