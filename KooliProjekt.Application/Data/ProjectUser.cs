using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string RoleInProject { get; set; }
    }
}
