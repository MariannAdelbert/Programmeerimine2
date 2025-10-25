using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Data
{
    public class ProjectTask
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
        public User ResponsibleUser { get; set; }

        public ICollection<WorkLog> WorkLogs { get; set; }
        public ICollection<TaskFile> TaskFiles { get; set; }
    }
}
