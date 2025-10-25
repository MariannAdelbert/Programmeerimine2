using System.Collections.Generic;

namespace KooliProjekt.Application.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<WorkLog> WorkLogs { get; set; }
    }
}
