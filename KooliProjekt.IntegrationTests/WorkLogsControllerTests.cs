using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.WorkLogs;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class WorkLogsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var user = new User
            {
                Name = "Alice",
                Email = "alice@example.com",
                Password = "Test123!",
                Role = "Developer"
            };
            await DbContext.AddAsync(user);

            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Task 1",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                Description = "Task description",
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var log1 = new WorkLog
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.UtcNow,
                HoursSpent = 2,
                Description = "Worked on task"
            };
            var log2 = new WorkLog
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.UtcNow,
                HoursSpent = 3,
                Description = "More work"
            };
            await DbContext.AddRangeAsync(log1, log2);
            await DbContext.SaveChangesAsync();

            var url = "/api/WorkLogs/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<WorkLogDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_worklog()
        {
            // Arrange
            var user = new User
            {
                Name = "Bob",
                Email = "bob@example.com",
                Password = "Test123!",
                Role = "Tester"
            };
            await DbContext.AddAsync(user);

            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Task 2",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 4,
                Description = "Task 2 description",
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var workLog = new WorkLog
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.UtcNow,
                HoursSpent = 5,
                Description = "Worked 5 hours"
            };
            await DbContext.AddAsync(workLog);
            await DbContext.SaveChangesAsync();

            var url = $"/api/WorkLogs/Get/{workLog.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<WorkLogDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(workLog.Id, response.Value.Id);
            Assert.Equal(workLog.Description, response.Value.Description);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_worklog()
        {
            // Arrange
            var missingId = 9999;
            var url = $"/api/WorkLogs/Get/{missingId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_worklog()
        {
            // 1️⃣ Loo ja salvesta projekt
            var project = new Project { Name = "Integration Project" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // 2️⃣ Loo ja salvesta kasutaja
            var user = new User { Email = "user@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // 3️⃣ Loo ja salvesta task, mis viitab projektile
            var task = new ProjectTask
            {
                Title = "Integration Task",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // 4️⃣ Loo WorkLog
            var command = new SaveWorkLogCommand
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.UtcNow,
                HoursSpent = 3,
                Description = "Worked on task"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/WorkLogs/Save")
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<OperationResult<WorkLogDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(task.Id, result.Value.TaskId);
            Assert.Equal(user.Id, result.Value.UserId);
        }

        [Fact]
        public async Task Delete_should_remove_existing_worklog()
        {
            // 1️⃣ Loo ja salvesta projekt
            var project = new Project { Name = "Project for Delete" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // 2️⃣ Loo ja salvesta kasutaja
            var user = new User { Email = "deleteuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // 3️⃣ Loo ja salvesta task
            var task = new ProjectTask
            {
                Title = "Task for Delete WorkLog",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // 4️⃣ Loo ja salvesta WorkLog
            var workLog = new WorkLog
            {
                TaskId = task.Id,
                UserId = user.Id,
                Date = DateTime.UtcNow,
                HoursSpent = 2,
                Description = "To be deleted"
            };
            await DbContext.AddAsync(workLog);
            await DbContext.SaveChangesAsync();

            var command = new DeleteWorkLogCommand { Id = workLog.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/WorkLogs/Delete")
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var deleted = await DbContext.WorkLogs.AsNoTracking().FirstOrDefaultAsync(w => w.Id == workLog.Id);
            Assert.Null(deleted);
        }
    }
}