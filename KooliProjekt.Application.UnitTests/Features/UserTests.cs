using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.User;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Users
{
    public class UserTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                Role = "Developer"
            };

            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new GetUserQuery { Id = user.Id };
            var handler = new GetUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5000)]
        public async Task Get_should_return_null_when_user_does_not_exist(int id)
        {
            var user = new User
            {
                Name = "Existing User",
                Email = "existing@example.com",
                Password = "password",
                Role = "Tester"
            };

            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new GetUserQuery { Id = id };
            var handler = new GetUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetUserQuery query = null;
            var handler = new GetUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // ===== List Tests =====
        [Fact]
        public async Task List_should_return_users_page()
        {
            for (int i = 1; i <= 10; i++)
            {
                await DbContext.Users.AddAsync(new User
                {
                    UserName = $"user{i}",
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    Password = "Password123",
                    Role = "Developer"
                });
            }
            await DbContext.SaveChangesAsync();

            var query = new ListUsersQuery { Page = 1, PageSize = 5 };
            var handler = new ListUsersQueryHandler(DbContext);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.CurrentPage);
            Assert.Equal(5, result.Value.Results.Count);
        }

        // ===== Save Tests =====
        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var command = new SaveUserCommand
            {
                UserName = "userNew",
                Name = "New User",
                Email = "new@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Users.FirstOrDefaultAsync(); // võta esimene kirje

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal("New User", saved.Name);
            Assert.Equal("new@test.com", saved.Email);
            Assert.Equal("Developer", saved.Role);
        }

        [Fact]
        public async Task Save_should_update_existing_user()
        {
            var existing = new User
            {
                UserName = "userOld",
                Name = "Old User",
                Email = "old@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            await DbContext.Users.AddAsync(existing);
            await DbContext.SaveChangesAsync();

            var command = new SaveUserCommand
            {
                Id = existing.Id,
                UserName = "userOld",
                Name = "Updated User",
                Email = "old@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var updated = await DbContext.Users.FindAsync(existing.Id);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal("Updated User", updated.Name);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<System.ArgumentNullException>(() =>
            {
                new DeleteUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_delete_existing_user()
        {
            // Arrange
            var user = new User
            {
                UserName = "user1",
                Name = "Delete Me",
                Email = "delete@test.com",
                Password = "Password123",
                Role = "Developer"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var command = new DeleteUserCommand { Id = user.Id };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Users.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(saved); // kasutaja kustutatud
        }

        [Fact]
        public async Task Delete_should_do_nothing_for_nonexistent_user()
        {
            // Arrange
            var command = new DeleteUserCommand { Id = 9999 }; // olemasolemata Id
            var handler = new DeleteUserCommandHandler(DbContext);

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
            var command = new DeleteUserCommand { Id = id };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}
