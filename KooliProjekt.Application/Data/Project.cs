using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public decimal HourlyRate { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
