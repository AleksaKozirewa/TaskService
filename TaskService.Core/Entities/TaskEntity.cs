using System.ComponentModel.DataAnnotations;

namespace TaskService.Core.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.New;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
