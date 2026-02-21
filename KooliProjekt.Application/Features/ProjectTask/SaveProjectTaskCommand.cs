using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public decimal EstimatedHours { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public decimal FixedPrice { get; set; }
        public int ResponsibleUserId { get; set; }
    }
}