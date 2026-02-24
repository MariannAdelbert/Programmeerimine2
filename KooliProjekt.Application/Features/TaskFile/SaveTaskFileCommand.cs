using MediatR;
using KooliProjekt.Application.Infrastructure.Results;
using System;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class SaveTaskFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}