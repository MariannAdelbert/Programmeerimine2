using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectTasksControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);

            var user = new User
            {
                Email = "testuser@example.com",
                Password = "Test123!"
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Lisame testülesanded
            var task1 = new ProjectTask
            {
                Title = "Task 1",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 5,
                Description = "Test description 1",
                IsCompleted = false,
                FixedPrice = 100,
                ResponsibleUserId = user.Id // ✅ FK korrektne
            };
            var task2 = new ProjectTask
            {
                Title = "Task 2",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 3,
                Description = "Test description 2",
                IsCompleted = false,
                FixedPrice = 50,
                ResponsibleUserId = user.Id // ✅ FK korrektne
            };
            await DbContext.AddRangeAsync(task1, task2);
            await DbContext.SaveChangesAsync();

            var url = $"/api/ProjectTasks/List?projectId={project.Id}&page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectTask>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_task()
        {
            // Arrange
            var project = new Project { Name = "Test project" };
            await DbContext.AddAsync(project);

            var user = new User
            {
                Email = "testuser2@example.com",
                Password = "Test123!"
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var task = new ProjectTask
            {
                Title = "Test Task",
                ProjectId = project.Id,
                StartDate = DateTime.UtcNow,
                EstimatedHours = 4,
                Description = "Task description",
                IsCompleted = false,
                FixedPrice = 75,
                ResponsibleUserId = user.Id // ✅ FK korrektne
            };
            await DbContext.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var url = $"/api/ProjectTasks/Get/{task.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectTask>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(task.Id, response.Value.Id);
            Assert.Equal(task.Title, response.Value.Title);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_task()
        {
            // Arrange
            var missingTaskId = 9999;
            var url = $"/api/ProjectTasks/Get/{missingTaskId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}