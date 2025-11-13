using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Role { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<WorkLog> WorkLogs { get; set; }
    }
}
