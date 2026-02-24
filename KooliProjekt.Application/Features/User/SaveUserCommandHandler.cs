using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.User;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult<UserDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<UserDto>> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            KooliProjekt.Application.Data.User entity;

            if (request.Id.HasValue)
            {
                entity = await _dbContext.Users.FindAsync(new object[] { request.Id.Value }, cancellationToken);
                if (entity == null)
                    return new OperationResult<UserDto>();
            }
            else
            {
                entity = new KooliProjekt.Application.Data.User
                {
                    UserName = request.UserName,
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Role = request.Role
                };
                await _dbContext.Users.AddAsync(entity, cancellationToken);
            }

            // Kui uuendame olemasolevat kasutajat, siis täidame väljad
            if (request.Id.HasValue)
            {
                entity.UserName = request.UserName;
                entity.Name = request.Name;
                entity.Email = request.Email;
                entity.Password = request.Password;
                entity.Role = request.Role;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult<UserDto>
            {
                Value = new UserDto
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    Name = entity.Name,
                    Email = entity.Email,
                    Role = entity.Role
                }
            };
        }
    }
}