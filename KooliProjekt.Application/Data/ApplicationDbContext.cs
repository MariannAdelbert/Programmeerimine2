using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

            modelBuilder.Entity<Project>()
                .Property(p => p.Budget)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Project>()
                .Property(p => p.HourlyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.EstimatedHours)
                .HasPrecision(18, 2);
            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.FixedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WorkLog>()
                .Property(w => w.HoursSpent)
                .HasPrecision(18, 2);
        }
    }
}
