using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Projects;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Projects
{
    public class ProjectTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_project()
        {
            // Arrange
            var project = new Project
            {
                Name = "Test project"
            };

            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectQuery { Id = project.Id };
            var handler = new GetProjectQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);

            var value = result.Value;
            Assert.Equal(project.Id,
                value.GetType().GetProperty("Id")?.GetValue(value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Get_should_return_null_when_project_does_not_exist(int id)
        {
            // Arrange
            var query = new GetProjectQuery { Id = id };
            var handler = new GetProjectQueryHandler(DbContext);

            var project = new Project { Name = "Test project" };
            await DbContext.Projects.AddAsync(project);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            // Arrange
            GetProjectQuery query = null;
            var handler = new GetProjectQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
