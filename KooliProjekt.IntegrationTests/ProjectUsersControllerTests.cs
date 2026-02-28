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
    public class ProjectUsersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);

            var user1 = new User { Email = "testuser1@example.com", Password = "Test123!" };
            var user2 = new User { Email = "testuser2@example.com", Password = "Test123!" };
            await DbContext.AddRangeAsync(user1, user2);
            await DbContext.SaveChangesAsync();

            var projectUser1 = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user1.Id,
                RoleInProject = "Developer"
            };
            var projectUser2 = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user2.Id,
                RoleInProject = "Tester"
            };
            await DbContext.AddRangeAsync(projectUser1, projectUser2);
            await DbContext.SaveChangesAsync();

            var url = $"/api/ProjectUsers/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectUserDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_project_user()
        {
            // Arrange
            var project = new Project { Name = "Test Project" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "testuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Developer"
            };
            await DbContext.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var url = $"/api/ProjectUsers/Get/{projectUser.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectUserDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(projectUser.Id, response.Value.Id);
            Assert.Equal(projectUser.RoleInProject, response.Value.RoleInProject);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_project_user()
        {
            // Arrange
            var missingId = 9999;
            var url = $"/api/ProjectUsers/Get/{missingId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}