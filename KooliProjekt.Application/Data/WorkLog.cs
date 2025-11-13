using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class WorkLog
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Description { get; set; }
    }
}
