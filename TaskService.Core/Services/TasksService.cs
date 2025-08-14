using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Core.Entities;
using TaskService.Core.Interfaces;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.Core.Services
{
    public class TasksService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TasksService> _logger;

        public TasksService(ITaskRepository repository, ILogger<TasksService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TaskEntity> CreateTaskAsync(string title, string description, DateTime dueDate)
        {
            var task = new TaskEntity
            {
                Title = title,
                Description = description,
                DueDate = dueDate.ToUniversalTime(),
                Status = TaskStatus.New
            };

            return await _repository.AddAsync(task);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync(int pageNumber, int pageSize, TaskStatus? status)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, status);
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TaskEntity> UpdateTaskAsync(Guid id, string title, string description, DateTime dueDate, TaskStatus status)
        {
            var task = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException();

            task.Title = title;
            task.Description = description;
            task.DueDate = dueDate.ToUniversalTime();
            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(task);
            return task;
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException();

            await _repository.DeleteAsync(id);
        }
    }
}
