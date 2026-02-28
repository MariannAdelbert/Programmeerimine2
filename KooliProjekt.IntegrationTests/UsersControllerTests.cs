using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.User;
using KooliProjekt.Application.Features.Users;
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

        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var url = "/api/Users/Save";
            var command = new SaveUserCommand
            {
                UserName = "testuser1",
                Name = "Test User",
                Email = "testuser1@example.com",
                Password = "Test123!",
                Role = "Developer"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<OperationResult<UserDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal("testuser1", result.Value.UserName);

            var dbUser = await DbContext.Users.FindAsync(result.Value.Id);
            Assert.NotNull(dbUser);
            Assert.Equal("testuser1@example.com", dbUser.Email);
        }

        [Fact]
        public async Task Delete_should_remove_existing_user()
        {
            // Arrange
            var user = new User
            {
                UserName = "deleteuser",
                Name = "Delete Me",
                Email = "deleteuser@example.com",
                Password = "Test123!",
                Role = "Tester"
            };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Users/Delete?id={user.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Uuesti DbContext’ist, AsNoTracking et saada puhas query
            var deletedUser = await DbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            Assert.Null(deletedUser); // ✅ Nüüd töötab
        }

        [Fact]
        public async Task Save_should_not_update_missing_user()
        {
            // Arrange
            var url = "/api/Users/Save";
            var command = new SaveUserCommand
            {
                Id = 9999,
                UserName = "ghostuser",
                Name = "Ghost",
                Email = "ghost@example.com",
                Password = "Ghost123!",
                Role = "Ghost"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}