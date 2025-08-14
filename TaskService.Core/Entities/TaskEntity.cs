using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Core.Entities
{
    public class TaskEntity
    {
        // Основные свойства
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        // Статус задачи с значением по умолчанию
        public TaskStatus Status { get; set; } = TaskStatus.New;

        // Метки времени
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
