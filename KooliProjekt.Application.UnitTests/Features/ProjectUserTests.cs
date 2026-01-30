using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.ProjectUsers;
using KooliProjekt.Application.Data;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.ProjectUsers
{
    public class ProjectUserTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectUserQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_project_user()
        {
            var projectUser = new ProjectUser
            {
                ProjectId = 1,
                UserId = 1,
                RoleInProject = "Developer"
            };

            await DbContext.ProjectUsers.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectUserQuery { Id = projectUser.Id };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(projectUser.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5000)]
        public async Task Get_should_return_null_when_project_user_does_not_exist(int id)

        {
            var projectUser = new ProjectUser
            {
                ProjectId = 1,
                UserId = 1,
                RoleInProject = "Tester"
            };

            await DbContext.ProjectUsers.AddAsync(projectUser);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectUserQuery { Id = id };
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetProjectUserQuery query = null;
            var handler = new GetProjectUserQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
