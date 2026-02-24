using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectUsers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;

namespace KooliProjekt.Application.UnitTests.Features.ProjectUsers
{
    public class ProjectUserTests : TestBase
    {
        // ===== Get Tests =====
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_project_user()
        {
            var project = new Project { Name = "Test Project" };
            var user = new User
            {
                UserName = "user1",
                Email = "user1@example.com",
                Name = "User One",
                Password = "Password123!",
                Role = "Developer"
            };

            await DbContext.Projects.AddAsync(project);
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Developer"
            };
            await DbContext.ProjectUsers.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectUserQuery { Id = projectUser.Id };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);

            Assert.Equal(projectUser.Id, result.Value.Id);
            Assert.Equal(project.Id, result.Value.ProjectId);
            Assert.Equal(user.Id, result.Value.UserId);
            Assert.Equal("Developer", result.Value.RoleInProject);
            Assert.True(projectUser.Id > 0);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        public async Task Get_should_return_null_when_project_user_does_not_exist(int id)
        {
            var project = new Project { Name = "Test Project" };
            var user = new User
            {
                UserName = "user2",
                Email = "user2@example.com",
                Name = "User Two",
                Password = "Password123!",
                Role = "Tester"
            };
            await DbContext.Projects.AddAsync(project);
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectUserQuery { Id = id };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetProjectUserQuery query = null;
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // ===== List Tests =====
        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListProjectUsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListProjectUsersQuery)null;
            var handler = new ListProjectUsersQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 0)]
        [InlineData(-1, 5)]
        public async Task List_should_return_null_when_page_or_page_size_is_invalid(int page, int pageSize)
        {
            var query = new ListProjectUsersQuery { Page = page, PageSize = pageSize };
            var handler = new ListProjectUsersQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_project_users()
        {
            for (int i = 1; i <= 15; i++)
            {
                var project = new Project { Name = $"Project {i}" };
                var user = new User
                {
                    UserName = $"user{i}",
                    Email = $"user{i}@example.com",
                    Name = $"User {i}",
                    Password = "Password123!",
                    Role = "Developer"
                };
                await DbContext.Projects.AddAsync(project);
                await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();

                var pu = new ProjectUser
                {
                    ProjectId = project.Id,
                    UserId = user.Id,
                    RoleInProject = $"Role {i}"
                };
                await DbContext.ProjectUsers.AddAsync(pu);
            }
            await DbContext.SaveChangesAsync();

            var query = new ListProjectUsersQuery { Page = 2, PageSize = 5 };
            var handler = new ListProjectUsersQueryHandler(DbContext);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.CurrentPage);
            Assert.Equal(5, result.Value.Results.Count);
        }

        // ===== Save Tests =====
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveProjectUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var request = (SaveProjectUserCommand)null;
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_add_new_project_user()
        {
            var project = new Project { Name = "Project1" };
            var user = new User
            {
                UserName = "User1",
                Email = "user1@example.com",
                Name = "User One",
                Password = "Password123!",
                Role = "Developer"
            };
            await DbContext.Projects.AddAsync(project);
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new SaveProjectUserCommand
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Manager"
            };
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);
            var saved = await DbContext.ProjectUsers.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal("Manager", saved.RoleInProject);
        }

        [Fact]
        public async Task Save_should_update_existing_project_user()
        {
            var project = new Project { Name = "Project2" };
            var user = new User
            {
                UserName = "User2",
                Email = "user2@example.com",
                Name = "User Two",
                Password = "Password123!",
                Role = "Developer"
            };
            await DbContext.Projects.AddAsync(project);
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var existing = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Old Role"
            };
            await DbContext.ProjectUsers.AddAsync(existing);
            await DbContext.SaveChangesAsync();

            var query = new SaveProjectUserCommand
            {
                Id = existing.Id,
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "New Role"
            };
            var handler = new SaveProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            // Clear tracker to avoid duplicate key tracking error
            DbContext.ChangeTracker.Clear();

            var updated = await DbContext.ProjectUsers.FirstOrDefaultAsync(pu => pu.ProjectId == project.Id && pu.UserId == user.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal("New Role", updated.RoleInProject);
        }

        // ===== Delete Tests =====
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteProjectUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var request = (DeleteProjectUserCommand)null;
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 5)]
        [InlineData(5, -10)]
        public async Task Delete_should_not_fail_when_projectuser_keys_are_invalid(int projectId, int userId)
        {
            var command = new DeleteProjectUserCommand
            {
                ProjectId = projectId,
                UserId = userId
            };
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_remove_existing_project_user()
        {
            var project = new Project { Name = "Project 1" };
            var user = new User
            {
                UserName = "User1",
                Email = "user1@example.com",
                Name = "User One",
                Password = "Password123!",
                Role = "Developer"
            };
            await DbContext.Projects.AddAsync(project);
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Developer"
            };
            await DbContext.ProjectUsers.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var command = new DeleteProjectUserCommand
            {
                ProjectId = project.Id,
                UserId = user.Id
            };
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            var count = await DbContext.ProjectUsers.CountAsync();
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_project_user()
        {
            var command = new DeleteProjectUserCommand
            {
                ProjectId = 9999,
                UserId = 8888
            };
            var handler = new DeleteProjectUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // -------------------
        // SAVE PROJECT USER VALIDATOR TESTS
        // -------------------

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SaveUserValidator_should_fail_when_project_id_is_invalid(int projectId)
        {
            // Arrange
            var command = new SaveProjectUserCommand
            {
                ProjectId = projectId,
                UserId = 1,
                RoleInProject = "Valid role"
            };
            var validator = new SaveProjectUserCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectUserCommand.ProjectId), error.PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task SaveUserValidator_should_fail_when_user_id_is_invalid(int userId)
        {
            // Arrange
            var command = new SaveProjectUserCommand
            {
                ProjectId = 1,
                UserId = userId,
                RoleInProject = "Valid role"
            };
            var validator = new SaveProjectUserCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectUserCommand.UserId), error.PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789001")] // >50 tähemärki
        public async Task SaveUserValidator_should_fail_when_role_is_invalid(string role)
        {
            // Arrange
            var command = new SaveProjectUserCommand
            {
                ProjectId = 1,
                UserId = 1,
                RoleInProject = role
            };
            var validator = new SaveProjectUserCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectUserCommand.RoleInProject), error.PropertyName);
        }

        [Fact]
        public async Task SaveUserValidator_should_succeed_when_command_is_valid()
        {
            // Arrange
            var command = new SaveProjectUserCommand
            {
                ProjectId = 1,
                UserId = 1,
                RoleInProject = "Developer"
            };
            var validator = new SaveProjectUserCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}