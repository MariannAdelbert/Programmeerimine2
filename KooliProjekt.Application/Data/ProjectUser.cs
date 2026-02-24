using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class ProjectUser
    {
        public int Id { get; set; }   // ✅ Primary Key

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
