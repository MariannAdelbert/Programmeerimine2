using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TaskFiles;
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

        [Fact]
        public async Task Save_should_add_new_taskfile()
        {
            // Arrange
            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "testuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Test Task",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 3,
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var url = "/api/TaskFiles/Save";

            var command = new SaveTaskFileCommand
            {
                Id = 0,
                TaskId = task.Id,
                FileName = "TestFile.txt",
                FilePath = "/files/TestFile.txt",
                UploadDate = DateTime.UtcNow
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.False(result.HasErrors);

            var dbFile = await DbContext.TaskFiles
    .FirstOrDefaultAsync(tf => tf.FileName == command.FileName && tf.TaskId == command.TaskId);

            Assert.NotNull(dbFile);
            Assert.Equal("TestFile.txt", dbFile.FileName);
        }

        [Fact]
        public async Task Delete_should_remove_existing_taskfile()
        {
            // Arrange
            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "testuser2@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Test Task 2",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                ResponsibleUserId = user.Id
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var taskFile = new TaskFile
            {
                TaskId = task.Id,
                FileName = "DeleteMe.txt",
                FilePath = "/files/DeleteMe.txt",
                UploadDate = DateTime.UtcNow
            };
            await DbContext.AddAsync(taskFile);
            await DbContext.SaveChangesAsync();

            var url = "/api/TaskFiles/Delete";
            var command = new DeleteTaskFileCommand { Id = taskFile.Id };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Kontrollime DB-st File olemasolu nimi ja TaskId järgi
            var deletedFile = await DbContext.TaskFiles
                .FirstOrDefaultAsync(tf => tf.Id == taskFile.Id);
            Assert.Null(deletedFile);
        }
    }
}