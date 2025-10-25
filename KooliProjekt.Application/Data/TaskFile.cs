using System;

namespace KooliProjekt.Application.Data
{
    public class TaskFile
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
