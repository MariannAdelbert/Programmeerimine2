using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TaskFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.TaskFiles
{
    public class TaskFileTests : TestBase
    {
        // ===== Get Tests =====
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
            var task = new ProjectTask { Title = "Task 1", StartDate = DateTime.Now };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var file = new TaskFile
            {
                TaskId = task.Id,
                FileName = "test.txt",
                FilePath = "/files/test.txt",
                UploadDate = DateTime.UtcNow
            };
            await DbContext.TaskFiles.AddAsync(file);
            await DbContext.SaveChangesAsync();

            var query = new GetTaskFileQuery { Id = file.Id };
            var handler = new GetTaskFileQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);

            // Cast DTO
            var value = (TaskFileDto)result.Value;
            Assert.Equal(file.Id, value.Id);
            Assert.Equal(task.Id, value.TaskId);
            Assert.Equal("test.txt", value.FileName);
            Assert.Equal("/files/test.txt", value.FilePath);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        public async Task Get_should_return_null_when_task_file_does_not_exist(int id)
        {
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

        // ===== List Tests =====
        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListTaskFilesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            var request = (ListTaskFilesQuery)null;
            var handler = new ListTaskFilesQueryHandler(DbContext);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });

            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 0)]
        [InlineData(-1, 5)]
        public async Task List_should_return_null_when_page_or_page_size_is_invalid(int page, int pageSize)
        {
            var query = new ListTaskFilesQuery { Page = page, PageSize = pageSize };
            var handler = new ListTaskFilesQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_task_files()
        {
            for (int i = 1; i <= 15; i++)
            {
                var task = new ProjectTask { Title = $"Task {i}", StartDate = DateTime.Now };
                await DbContext.ProjectTasks.AddAsync(task);
                await DbContext.SaveChangesAsync();

                var file = new TaskFile
                {
                    TaskId = task.Id,
                    FileName = $"file{i}.txt",
                    FilePath = $"/files/file{i}.txt",
                    UploadDate = DateTime.UtcNow
                };
                await DbContext.TaskFiles.AddAsync(file);
            }

            await DbContext.SaveChangesAsync();

            var query = new ListTaskFilesQuery { Page = 2, PageSize = 5 };
            var handler = new ListTaskFilesQueryHandler(DbContext);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.CurrentPage);
            Assert.Equal(5, result.Value.Results.Count);
            Assert.Equal(15, result.Value.TotalCount);
        }

        // ===== Delete Tests =====
        [Fact]
        public async Task Delete_should_remove_existing_task_file()
        {
            var task = new ProjectTask { Title = "Task Delete", StartDate = DateTime.Now };
            await DbContext.ProjectTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();

            var file = new TaskFile
            {
                TaskId = task.Id,
                FileName = "delete.txt",
                FilePath = "/files/delete.txt",
                UploadDate = DateTime.UtcNow
            };
            await DbContext.TaskFiles.AddAsync(file);
            await DbContext.SaveChangesAsync();

            var command = new DeleteTaskFileCommand { Id = file.Id };
            var handler = new DeleteTaskFileCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            var count = await DbContext.TaskFiles.CountAsync();

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_not_fail_for_non_existing_file()
        {
            var command = new DeleteTaskFileCommand { Id = 9999 };
            var handler = new DeleteTaskFileCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // -------------------
        // TASKID TEST
        // -------------------
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task SaveFileValidator_should_fail_when_task_id_is_invalid(int taskId)
        {
            // ------------------- Arrange -------------------
            var command = new SaveTaskFileCommand
            {
                TaskId = taskId,
                FileName = "validfile.txt",
                FilePath = "/files/validfile.txt",
                UploadDate = DateTime.Now
            };
            var validator = new SaveTaskFileCommandValidator();

            // ------------------- Act -------------------
            var result = await validator.ValidateAsync(command);

            // ------------------- Assert -------------------
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveTaskFileCommand.TaskId), error.PropertyName);
        }

        // -------------------
        // FILENAME TEST
        // -------------------
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567878945613290123456789012341235678901234567890123456789012345678901234567890123456789012345678901")] // 101 tähemärki
        public async Task SaveFileValidator_should_fail_when_file_name_is_invalid(string fileName)
        {
            // ------------------- Arrange -------------------
            var command = new SaveTaskFileCommand
            {
                TaskId = 1,
                FileName = fileName,
                FilePath = "/files/file.txt",
                UploadDate = DateTime.Now
            };
            var validator = new SaveTaskFileCommandValidator();

            // ------------------- Act -------------------
            var result = await validator.ValidateAsync(command);

            // ------------------- Assert -------------------
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveTaskFileCommand.FileName), error.PropertyName);
        }

        // -------------------
        // FILEPATH TEST
        // -------------------
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("C:/very/long/path/that/exceeds/the/allowed/limit/for/file/path/and/keeps/going/on/forever/still/going/on/hello/its/me/is/it/long/now/1234567890.txt")] // >200 char
        public async Task SaveFileValidator_should_fail_when_file_path_is_invalid(string filePath)
        {
            // ------------------- Arrange -------------------
            var command = new SaveTaskFileCommand
            {
                TaskId = 1,
                FileName = "validfile.txt",
                FilePath = filePath,
                UploadDate = DateTime.Now
            };
            var validator = new SaveTaskFileCommandValidator();

            // ------------------- Act -------------------
            var result = await validator.ValidateAsync(command);

            // ------------------- Assert -------------------
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveTaskFileCommand.FilePath), error.PropertyName);
        }

        // -------------------
        // UPLOADDATE TEST
        // -------------------
        [Fact]
        public async Task SaveFileValidator_should_fail_when_upload_date_in_future()
        {
            // ------------------- Arrange -------------------
            var command = new SaveTaskFileCommand
            {
                TaskId = 1,
                FileName = "validfile.txt",
                FilePath = "/files/validfile.txt",
                UploadDate = DateTime.Now.AddDays(1) // tulevikus
            };
            var validator = new SaveTaskFileCommandValidator();

            // ------------------- Act -------------------
            var result = await validator.ValidateAsync(command);

            // ------------------- Assert -------------------
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            var error = result.Errors.First();
            Assert.Equal(nameof(SaveTaskFileCommand.UploadDate), error.PropertyName);
        }

        // -------------------
        // POSITIIVNE TEST
        // -------------------
        [Fact]
        public async Task SaveFileValidator_should_succeed_when_command_is_valid()
        {
            // ------------------- Arrange -------------------
            var command = new SaveTaskFileCommand
            {
                TaskId = 1,
                FileName = "validfile.txt",
                FilePath = "/files/validfile.txt",
                UploadDate = DateTime.Now
            };
            var validator = new SaveTaskFileCommandValidator();

            // ------------------- Act -------------------
            var result = await validator.ValidateAsync(command);

            // ------------------- Assert -------------------
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}