using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Data;
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
    }
}
