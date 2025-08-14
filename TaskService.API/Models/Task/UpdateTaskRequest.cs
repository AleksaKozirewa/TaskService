using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.API.Models.Task
{
    public class UpdateTaskRequest : CreateTaskRequest
    {
        [Required]
        public TaskStatus Status { get; set; }
    }
}
