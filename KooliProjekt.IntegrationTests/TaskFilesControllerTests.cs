using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TaskFilesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "user@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Test Task",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                Description = "Description",
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            // Lisame TaskFile kirjed
            var file1 = new TaskFile
            {
                TaskId = task.Id, // ✅ õige FK
                FileName = "file1.txt",
                FilePath = "/files/file1.txt",
                UploadDate = DateTime.UtcNow
            };
            var file2 = new TaskFile
            {
                TaskId = task.Id,
                FileName = "file2.txt",
                FilePath = "/files/file2.txt",
                UploadDate = DateTime.UtcNow
            };
            await DbContext.AddRangeAsync(file1, file2);
            await DbContext.SaveChangesAsync();

            var url = "/api/TaskFiles/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<TaskFileDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_task_file()
        {
            // Arrange
            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "user2@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Test Task",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                Description = "Description",
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var file = new TaskFile
            {
                TaskId = task.Id, // ✅ õige FK
                FileName = "file.txt",
                FilePath = "/files/file.txt",
                UploadDate = DateTime.UtcNow
            };
            await DbContext.AddAsync(file);
            await DbContext.SaveChangesAsync();

            var url = $"/api/TaskFiles/Get/{file.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<TaskFileDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(file.Id, response.Value.Id);
            Assert.Equal(file.FileName, response.Value.FileName);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_task_file()
        {
            // Arrange
            var missingId = 9999;
            var url = $"/api/TaskFiles/Get/{missingId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}