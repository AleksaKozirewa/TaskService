using System.ComponentModel.DataAnnotations;

namespace TaskService.API.Models.Task
{
    public class CreateTaskRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
