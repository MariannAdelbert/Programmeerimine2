using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public decimal EstimatedHours { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public decimal FixedPrice { get; set; }
        public int ResponsibleUserId { get; set; }
        public User ResponsibleUser { get; set; }

        public ICollection<WorkLog> WorkLogs { get; set; }
        public ICollection<TaskFile> TaskFiles { get; set; }
    }
}
