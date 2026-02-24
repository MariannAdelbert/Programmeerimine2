using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.WorkLogs
{
    [ExcludeFromCodeCoverage]
    public class SaveWorkLogCommand : IRequest<OperationResult<WorkLogDto>>
    {
        public int? Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }
        public string Description { get; set; }
    }
}