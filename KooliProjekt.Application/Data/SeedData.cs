using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IList<User> _users = new List<User>();
        private readonly IList<Project> _projects = new List<Project>();
        private readonly IList<ProjectTask> _tasks = new List<ProjectTask>();

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public void Generate()
        {
            // Kui kasutajad juba olemas, ära tee midagi
            if (_dbContext.Users.Any())
                return;

            GenerateUsers();
            GenerateProjects();
            _dbContext.SaveChanges();

            GenerateProjectUsers();
            GenerateProjectTasks();

            _dbContext.SaveChanges();

            GenerateWorkLogs();
            GenerateTaskFiles();

            _dbContext.SaveChanges();
        }

        private void GenerateUsers()
        {
            for (int i = 1; i <= 5; i++)
            {
                var user = new User
                {
                    Name = $"User {i}",
                    Email = $"user{i}@mail.com",
                    Password = "pass123",
                    Role = "Developer"
                };
                _users.Add(user);
            }

            _dbContext.Users.AddRange(_users);
        }

        private void GenerateProjects()
        {
            for (int i = 1; i <= 3; i++)
            {
                var project = new Project
                {
                    Name = $"Project {i}",
                    StartDate = DateTime.Now.AddDays(-10),
                    Deadline = DateTime.Now.AddDays(30),
                    Budget = 5000 + i * 1000,
                    HourlyRate = 25 + i * 5
                };

                _dbContext.Projects.Add(project);  // Lisa otse DbContext-i
                _projects.Add(project);             // Salvesta ka listi hilisemaks kasutamiseks
            }

            _dbContext.SaveChanges(); // nüüd on ID-d olemas
        }


        private void GenerateProjectUsers()
        {
            var rand = new Random();
            foreach (var project in _projects)
            {
                var assignedUsers = _users.OrderBy(u => rand.Next()).Take(3).ToList();
                foreach (var user in assignedUsers)
                {
                    _dbContext.ProjectUsers.Add(new ProjectUser
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        RoleInProject = "Member"
                    });
                }
            }
        }

        private void GenerateProjectTasks()
        {
            var rand = new Random();
            foreach (var project in _projects)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var responsibleUser = _users[rand.Next(_users.Count)];
                    var task = new ProjectTask
                    {
                        ProjectId = project.Id,
                        ResponsibleUserId = responsibleUser.Id,
                        Title = $"Task {i} for {project.Name}",
                        Description = "Automatically generated task",
                        EstimatedHours = rand.Next(5, 20),
                        FixedPrice = rand.Next(100, 300),
                        StartDate = DateTime.Now.AddDays(-rand.Next(1, 10)),
                        IsCompleted = false
                    };
                    _tasks.Add(task);
                }
            }

            _dbContext.ProjectTasks.AddRange(_tasks);
        }

        private void GenerateWorkLogs()
        {
            var rand = new Random();
            foreach (var task in _tasks)
            {
                int entries = rand.Next(1, 4);
                for (int i = 0; i < entries; i++)
                {
                    var user = _users[rand.Next(_users.Count)];
                    _dbContext.WorkLogs.Add(new WorkLog
                    {
                        TaskId = task.Id,
                        UserId = user.Id,
                        Date = DateTime.Now.AddDays(-rand.Next(1, 5)),
                        HoursSpent = rand.Next(1, 5),
                        Description = "Work log entry"
                    });
                }
            }
        }

        private void GenerateTaskFiles()
        {
            foreach (var task in _tasks)
            {
                _dbContext.TaskFiles.Add(new TaskFile
                {
                    TaskId = task.Id,
                    FileName = "example.txt",
                    FilePath = "uploads/example.txt",
                    UploadDate = DateTime.Now
                });
            }
        }
    }
}
