using System.ComponentModel.DataAnnotations;

namespace TaskService.API.Models.Task
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Заголовок задачи обязателен")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Заголовок должен быть от 3 до 100 символов")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата выполнения обязательна")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "Дата выполнения должна быть в будущем")]
        public DateTime DueDate { get; set; }

        // Кастомный атрибут валидации для проверки даты в будущем
        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime date)
                {
                    return date > DateTime.UtcNow;
                }
                return false;
            }
        }
    }
}
