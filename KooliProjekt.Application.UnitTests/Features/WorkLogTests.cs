using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.WorkLogs;
using KooliProjekt.Application.Data;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.WorkLogs
{
    public class WorkLogTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetWorkLogQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_worklog()
        {
            var worklog = new WorkLog
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 5,
                Description = "Worked on task"
            };

            await DbContext.WorkLogs.AddAsync(worklog);
            await DbContext.SaveChangesAsync();

            var query = new GetWorkLogQuery { Id = worklog.Id };
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(worklog.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5000)]
        public async Task Get_should_return_null_when_worklog_does_not_exist(int id)
        {
            var worklog = new WorkLog
            {
                TaskId = 1,
                UserId = 1,
                Date = DateTime.Now,
                HoursSpent = 3,
                Description = "Existing worklog"
            };

            await DbContext.WorkLogs.AddAsync(worklog);
            await DbContext.SaveChangesAsync();

            var query = new GetWorkLogQuery { Id = id };
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetWorkLogQuery query = null;
            var handler = new GetWorkLogQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
