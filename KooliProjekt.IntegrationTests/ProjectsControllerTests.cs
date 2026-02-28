using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Projects;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var url = "/api/Projects/List?page=1&pageSize=4";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Project>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_existing_list()
        {
            // Arrange
            var url = "/api/Projects/Get?id=1";

            var project = new Project { Name = "Test list" };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ProjectDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(1, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_list()
        {
            // Arrange
            var url = "/api/Projects/Get?id=134";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_create_new_project()
        {
            // Arrange
            var url = "/api/Projects/Save";

            var command = new
            {
                Id = 0,
                Name = "Integration Test Project",
                StartDate = DateTime.Today,
                Deadline = DateTime.Today.AddDays(30),
                Budget = 10000,
                HourlyRate = 50
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(result.HasErrors);

            var project = DbContext.Projects.FirstOrDefault(p => p.Name == "Integration Test Project");
            Assert.NotNull(project);
        }

        [Fact]
        public async Task Save_should_not_update_missing_project()
        {
            // Arrange
            var url = "/api/Projects/Save";

            var command = new SaveProjectCommand
            {
                Id = 999,
                Name = "Updated project",
                StartDate = DateTime.Today,
                Deadline = DateTime.Today.AddDays(10),
                Budget = 2000,
                HourlyRate = 60
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
        public async Task Delete_should_remove_existing_project()
        {
            var project = new Project
            {
                Name = "Delete project",
                StartDate = DateTime.Today,
                Deadline = DateTime.Today.AddDays(5),
                Budget = 500,
                HourlyRate = 25
            };
            await DbContext.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Projects/Delete?id={project.Id}";
            var response = await Client.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Kontroll API kaudu, mitte otse DbContext
            var getUrl = $"/api/Projects/Get?id={project.Id}";
            var getResponse = await Client.GetAsync(getUrl);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_should_handle_missing_project()
        {
            // Arrange
            var url = "/api/Projects/Delete";

            // Act
            var command = new DeleteProjectCommand { Id = 9999 };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            var response = await Client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
