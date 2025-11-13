using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class TaskFile
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
