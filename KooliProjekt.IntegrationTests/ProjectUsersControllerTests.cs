using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.ProjectUsers;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectUsersControllerTests : TestBase
    {
        private string url;

        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var project = new Project { Name = "Test Project for List" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "listuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync(); // oluline, et Id-d tekiksid

            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Developer"
            };
            await DbContext.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var url = $"/api/ProjectUsers/List?projectId={project.Id}";

            // Act
            var listResponse = await Client.GetFromJsonAsync<OperationResult<PagedResult<ProjectUserDto>>>(url);

            // Assert
            Assert.NotNull(listResponse);
            Assert.NotNull(listResponse.Value);
            Assert.False(listResponse.HasErrors);

            // Kontrollime, et lisatud user on olemas
            Assert.Contains(listResponse.Value.Results, pu => pu.UserId == user.Id && pu.ProjectId == project.Id);
        }

        [Fact]
        public async Task Save_should_add_new_user_to_project()
        {
            // Arrange
            var project = new Project { Name = "Project for Save Test" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "saveuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = "/api/ProjectUsers/Save";
            var command = new SaveProjectUserCommand
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Tester"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Save_should_not_update_missing_user()
        {
            // Arrange
            var url = "/api/ProjectUsers/Save";
            var command = new SaveProjectUserCommand
            {
                Id = 999, // mitteeksisteeriv
                ProjectId = 1,
                UserId = 1,
                RoleInProject = "NonExistent"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_user()
        {
            // Arrange
            var project = new Project { Name = "Project for Delete Test" };
            await DbContext.AddAsync(project);

            var user = new User { Email = "deleteuser@example.com", Password = "Test123!" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var projectUser = new ProjectUser
            {
                ProjectId = project.Id,
                UserId = user.Id,
                RoleInProject = "Tester"
            };
            await DbContext.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var url = "/api/ProjectUsers/Delete";
            var command = new DeleteProjectUserCommand
            {
                ProjectId = project.Id,
                UserId = user.Id
            };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_handle_missing_user()
        {
            // Arrange
            var url = "/api/ProjectUsers/Delete";
            var command = new DeleteProjectUserCommand
            {
                ProjectId = 9999,
                UserId = 8888
            };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}