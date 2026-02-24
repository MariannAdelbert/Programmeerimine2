using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.WorkLogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.WorkLogs
{
    public class WorkLogTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetWorkLogQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_worklog()
        {
            var worklog = new WorkLog
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 5,
                Description = "Worked on task"
            };

            await DbContext.WorkLogs.AddAsync(worklog);
            await DbContext.SaveChangesAsync();

            var query = new GetWorkLogQuery { Id = worklog.Id };
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(worklog.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5000)]
        public async Task Get_should_return_null_when_worklog_does_not_exist(int id)
        {
            var worklog = new WorkLog
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 3,
                Description = "Existing worklog"
            };

            await DbContext.WorkLogs.AddAsync(worklog);
            await DbContext.SaveChangesAsync();

            var query = new GetWorkLogQuery { Id = id };
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetWorkLogQuery query = null;
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // ===== Save Tests =====
        [Fact]
        public async Task Save_should_add_new_worklog()
        {
            var user = new User
            {
                UserName = "user2",
                Name = "User Two",
                Email = "user2@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            var task = new ProjectTask
            {
                Title = "Task 2",
                Description = "Test Task 2"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var command = new SaveWorkLogCommand
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.Today,
                HoursSpent = 4,
                Description = "Worked full day"
            };
            var handler = new SaveWorkLogCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.WorkLogs.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(4, saved.HoursSpent);
        }

        [Fact]
        public async Task Save_should_update_existing_worklog()
        {
            var user = new User
            {
                UserName = "user3",
                Name = "User Three",
                Email = "user3@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            var task = new ProjectTask
            {
                Title = "Task 3",
                Description = "Test Task 3"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var existing = new WorkLog
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.Today,
                HoursSpent = 1.5m,
                Description = "Initial work"
            };
            await DbContext.WorkLogs.AddAsync(existing);
            await DbContext.SaveChangesAsync();

            var command = new SaveWorkLogCommand
            {
                Id = existing.Id,
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.Today,
                HoursSpent = 3.0m,
                Description = "Updated work"
            };
            var handler = new SaveWorkLogCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var updated = await DbContext.WorkLogs.FindAsync(existing.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(3.0m, updated.HoursSpent);
            Assert.Equal("Updated work", updated.Description);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<System.ArgumentNullException>(() =>
            {
                new DeleteWorkLogCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_delete_existing_worklog()
        {
            // Arrange
            var workLog = new WorkLog
            {
                HoursSpent = 5,
                Description = "Test worklog",
            };
            await DbContext.WorkLogs.AddAsync(workLog);
            await DbContext.SaveChangesAsync();

            var command = new DeleteWorkLogCommand { Id = workLog.Id };
            var handler = new DeleteWorkLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.WorkLogs.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(saved); // WorkLog kustutatud
        }

        [Fact]
        public async Task Delete_should_do_nothing_for_nonexistent_worklog()
        {
            // Arrange
            var command = new DeleteWorkLogCommand { Id = 9999 }; // olemasolemata Id
            var handler = new DeleteWorkLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_should_ignore_zero_or_negative_id(int id)
        {
            // Arrange
            var command = new DeleteWorkLogCommand { Id = id };
            var handler = new DeleteWorkLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // -------------------
        // TASKID TEST
        // -------------------
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task SaveWorkLogValidator_should_fail_when_task_id_is_invalid(int taskId)
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = taskId,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 8,
                Description = "Valid description"
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveWorkLogCommand.TaskId), error.PropertyName);
        }

        // -------------------
        // USERID TEST
        // -------------------
        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public async Task SaveWorkLogValidator_should_fail_when_user_id_is_invalid(int userId)
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = 1,
                UserId = userId,
                Date = DateTime.Now,
                HoursSpent = 8,
                Description = "Valid description"
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveWorkLogCommand.UserId), error.PropertyName);
        }

        // -------------------
        // DATE TEST
        // -------------------
        [Fact]
        public async Task SaveWorkLogValidator_should_fail_when_date_is_in_future()
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now.AddDays(1), // tulevikus
                HoursSpent = 8,
                Description = "Valid description"
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveWorkLogCommand.Date), error.PropertyName);
        }

        // -------------------
        // HOURS SPENT TEST
        // -------------------
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(25)] // liiga palju
        public async Task SaveWorkLogValidator_should_fail_when_hours_spent_is_invalid(decimal hours)
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = hours,
                Description = "Valid description"
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveWorkLogCommand.HoursSpent), error.PropertyName);
        }

        // -------------------
        // DESCRIPTION TEST
        // -------------------
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vestibulum scelerisque quam at erat venenatis, a blandit turpis tincidunt.")] // >200 char
        public async Task SaveWorkLogValidator_should_fail_when_description_is_invalid(string description)
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 8,
                Description = description
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveWorkLogCommand.Description), error.PropertyName);
        }

        // -------------------
        // POSITIIVNE TEST
        // -------------------
        [Fact]
        public async Task SaveWorkLogValidator_should_succeed_when_command_is_valid()
        {
            // Arrange
            var command = new SaveWorkLogCommand
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 8,
                Description = "Valid description"
            };
            var validator = new SaveWorkLogCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}
