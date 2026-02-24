using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTasks;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.ProjectTasks
{
    public class ProjectTaskTests : TestBase
    {
        #region Get Tests

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectTaskQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_project_task()
        {
            var task = new ProjectTask
            {
                Title = "Test Task",
                Description = "Test description",
                StartDate = DateTime.Now,
                EstimatedHours = 10,
                IsCompleted = false,
                FixedPrice = 100,
                ResponsibleUserId = 1
            };

            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectTaskQuery { Id = task.Id };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task Get_should_return_null_when_task_does_not_exist(int id)
        {
            var query = new GetProjectTaskQuery { Id = id };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var task = new ProjectTask { Title = "Existing Task", Description = "Desc", StartDate = DateTime.Now };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetProjectTaskQuery query = null;
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        #endregion

        #region List Tests

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListProjectTasksQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListProjectTasksQuery)null;
            var handler = new ListProjectTasksQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(1, -10)]
        public async Task List_should_return_null_when_page_or_page_size_is_invalid(int page, int pageSize)
        {
            var query = new ListProjectTasksQuery { Page = page, PageSize = pageSize };
            var handler = new ListProjectTasksQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_tasks()
        {
            var query = new ListProjectTasksQuery { Page = 1, PageSize = 5 };
            var handler = new ListProjectTasksQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                var task = new ProjectTask
                {
                    Title = $"Task {i}",
                    Description = "Description",
                    StartDate = DateTime.Now
                };
                await DbContext.ProjectTasks.AddAsync(task);
            }
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        #endregion

        #region Save Tests

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveProjectTaskCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            var request = (SaveProjectTaskCommand)null;
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_add_new_task()
        {
            var command = new SaveProjectTaskCommand
            {
                Id = 0,
                Title = "New Task",
                Description = "Desc",
                StartDate = DateTime.Now,
                EstimatedHours = 5
            };
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.ProjectTasks.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(command.Title, saved.Title);
        }

        [Fact]
        public async Task Save_should_update_existing_task()
        {
            var task = new ProjectTask
            {
                Title = "Old Task",
                Description = "Old Desc",
                StartDate = DateTime.Now
            };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var command = new SaveProjectTaskCommand
            {
                Id = task.Id,
                Title = "Updated Task",
                Description = "Updated Desc",
                StartDate = task.StartDate
            };
            var handler = new SaveProjectTaskCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.ProjectTasks.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(command.Title, saved.Title);
            Assert.Equal(command.Description, saved.Description);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteProjectTaskCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            var request = (DeleteProjectTaskCommand)null;
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Delete_should_remove_existing_task()
        {
            var task = new ProjectTask
            {
                Title = "Task to delete",
                Description = "Desc",
                StartDate = DateTime.Now
            };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var command = new DeleteProjectTaskCommand { Id = task.Id };
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.ProjectTasks.Count();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_if_task_not_exist()
        {
            var command = new DeleteProjectTaskCommand { Id = 999 };
            var handler = new DeleteProjectTaskCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        #endregion

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789000")]
        public async Task SaveTaskValidator_should_fail_when_title_is_invalid(string title)
        {
            // Arrange
            var command = new SaveProjectTaskCommand { Title = title };
            var validator = new SaveProjectTaskCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectTaskCommand.Title), error.PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123890123456789012345678901238901234567890123456789012345678900123456789")]
        public async Task SaveTaskValidator_should_fail_when_description_is_invalid(string description)
        {
            // Arrange
            var command = new SaveProjectTaskCommand
            {
                Title = "Valid title",
                Description = description,
                ProjectId = 1,
                ResponsibleUserId = 1
            };
            var validator = new SaveProjectTaskCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectTaskCommand.Description), error.PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SaveTaskValidator_should_fail_when_project_id_is_invalid(int projectId)
        {
            // Arrange
            var command = new SaveProjectTaskCommand
            {
                Title = "Valid title",
                Description = "Valid description",
                ProjectId = projectId,
                ResponsibleUserId = 1
            };
            var validator = new SaveProjectTaskCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectTaskCommand.ProjectId), error.PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task SaveTaskValidator_should_fail_when_responsible_user_id_is_invalid(int userId)
        {
            // Arrange
            var command = new SaveProjectTaskCommand
            {
                Title = "Valid title",
                Description = "Valid description",
                ProjectId = 1,
                ResponsibleUserId = userId
            };

            var validator = new SaveProjectTaskCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);

            var error = result.Errors.First();
            Assert.Equal(nameof(SaveProjectTaskCommand.ResponsibleUserId), error.PropertyName);
        }

        [Fact]
        public async Task SaveTaskValidator_should_succeed_when_command_is_valid()
        {
            // Arrange
            var command = new SaveProjectTaskCommand
            {
                Title = "Task title",
                Description = "Task description",
                ProjectId = 1,
                ResponsibleUserId = 1
            };
            var validator = new SaveProjectTaskCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}