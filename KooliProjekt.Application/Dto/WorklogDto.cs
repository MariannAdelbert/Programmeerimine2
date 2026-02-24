using System;

namespace KooliProjekt.Application.Dto
{
    public class WorkLogDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }
        public string Description { get; set; }
    }
}