using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.TaskFiles;
using KooliProjekt.Application.Data;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.TaskFiles
{
    public class TaskFileTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetTaskFileQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_existing_task_file()
        {
            var file = new TaskFile
            {
                TaskId = 1,
                FileName = "test.txt",
                FilePath = "/files/test.txt",
                UploadDate = DateTime.Now
            };

            await DbContext.TaskFiles.AddAsync(file);
            await DbContext.SaveChangesAsync();

            var query = new GetTaskFileQuery { Id = file.Id };
            var handler = new GetTaskFileQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(file.Id, result.Value.GetType().GetProperty("Id")?.GetValue(result.Value));
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5000)]
        public async Task Get_should_return_null_when_task_file_does_not_exist(int id)
        {
            var file = new TaskFile
            {
                TaskId = 1,
                FileName = "existing.txt",
                FilePath = "/files/existing.txt",
                UploadDate = DateTime.Now
            };

            await DbContext.TaskFiles.AddAsync(file);
            await DbContext.SaveChangesAsync();

            var query = new GetTaskFileQuery { Id = id };
            var handler = new GetTaskFileQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_survive_null_request()
        {
            GetTaskFileQuery query = null;
            var handler = new GetTaskFileQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
