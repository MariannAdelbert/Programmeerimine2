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
    public class UsersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_positive_result()
        {
            // Arrange
            var user1 = new User
            {
                Name = "Alice",
                Email = "alice@example.com",
                Password = "Test123!",
                Role = "Developer"
            };
            var user2 = new User
            {
                Name = "Bob",
                Email = "bob@example.com",
                Password = "Test123!",
                Role = "Tester"
            };
            await DbContext.AddRangeAsync(user1, user2);
            await DbContext.SaveChangesAsync();

            var url = "/api/Users/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<UserDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.True(response.Value.Results.Count >= 2);
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            // Arrange
            var user = new User
            {
                Name = "Charlie",
                Email = "charlie@example.com",
                Password = "Test123!",
                Role = "Manager"
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Users/Get/{user.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<UserDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Value);
            Assert.False(response.HasErrors);
            Assert.Equal(user.Id, response.Value.Id);
            Assert.Equal(user.Name, response.Value.Name);
        }

        [Fact]
        public async Task Get_should_return_error_for_missing_user()
        {
            // Arrange
            var missingUserId = 9999;
            var url = $"/api/Users/Get/{missingUserId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}