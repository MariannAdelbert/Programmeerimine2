using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.ProjectTasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.ProjectTasks
{
    public class ProjectTaskTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetProjectTaskQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_project_task()
        {
            var task = new ProjectTask
            {
                Title = "Test Task",
                Description = "Test description",
                StartDate = DateTime.Now,
                EstimatedHours = 10,
                IsCompleted = false,
                FixedPrice = 100,
                ResponsibleUserId = 1
            };

            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var query = new GetProjectTaskQuery { Id = task.Id };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(task.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        public async Task Get_should_return_null_when_project_task_does_not_exist(int id)
        {
            var query = new GetProjectTaskQuery { Id = id };
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var task = new ProjectTask { Title = "Existing Task" };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetProjectTaskQuery query = null;
            var handler = new GetProjectTaskQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
