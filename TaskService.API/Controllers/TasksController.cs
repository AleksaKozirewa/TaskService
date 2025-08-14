using Microsoft.AspNetCore.Mvc;
using TaskService.API.Models.Task;
using TaskService.Core.Interfaces;
using TaskService.Core.Services;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] TaskStatus? status = null)
        {
            _logger.LogInformation("Запрос списка задач. Страница: {Page}, Размер страницы: {PageSize}, Статус: {Status}",
            page, pageSize, status);

            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            try
            {
                var tasks = await _taskService.GetTasksAsync(page, pageSize, status);
                _logger.LogInformation("Успешно получено {Count} задач", tasks.Count());
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка задач");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            _logger.LogInformation("Запрос задачи по ID: {TaskId}", id);

            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    _logger.LogWarning("Задача с ID {TaskId} не найдена", id);
                    return NotFound();
                }

                _logger.LogInformation("Успешно найдена задача с ID: {TaskId}", id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задачи с ID: {TaskId}", id);
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            _logger.LogInformation("Запрос на создание новой задачи. Заголовок: {Title}", request.Title);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некорректные данные запроса: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var task = await _taskService.CreateTaskAsync(
                    request.Title,
                    request.Description,
                    request.DueDate);

                _logger.LogInformation("Создана новая задача с ID: {TaskId}", task.Id);
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании задачи");
                return StatusCode(500, "Произошла ошибка при создании задачи");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
        {
            _logger.LogInformation("Запрос на обновление задачи с ID: {TaskId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некорректные данные запроса для задачи ID {TaskId}: {@ModelState}",
                    id, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var task = await _taskService.UpdateTaskAsync(
                    id,
                    request.Title,
                    request.Description,
                    request.DueDate,
                    request.Status);

                _logger.LogInformation("Задача с ID {TaskId} успешно обновлена", id);
                return Ok(task);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Попытка обновления несуществующей задачи с ID: {TaskId}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении задачи с ID: {TaskId}", id);
                return StatusCode(500, "Произошла ошибка при обновлении задачи");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            _logger.LogInformation("Запрос на удаление задачи с ID: {TaskId}", id);

            try
            {
                await _taskService.DeleteTaskAsync(id);
                _logger.LogInformation("Задача с ID {TaskId} успешно удалена", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Попытка удаления несуществующей задачи с ID: {TaskId}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении задачи с ID: {TaskId}", id);
                return StatusCode(500, "Произошла ошибка при удалении задачи");
            }
        }

        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            _logger.LogTrace("Trace message");
            _logger.LogDebug("Debug message");
            _logger.LogInformation("Information message");
            _logger.LogWarning("Warning message");
            _logger.LogError("Error message");
            return Ok("Test messages logged");
        }
    }
}
