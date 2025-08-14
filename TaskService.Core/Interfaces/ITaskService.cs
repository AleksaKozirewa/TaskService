using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Core.Entities;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string title, string description, DateTime dueDate);
        Task<IEnumerable<TaskEntity>> GetTasksAsync(int pageNumber, int pageSize, TaskStatus? status);
        Task<TaskEntity> GetTaskByIdAsync(Guid id);
        Task<TaskEntity> UpdateTaskAsync(Guid id, string title, string description, DateTime dueDate, TaskStatus status);
        Task DeleteTaskAsync(Guid id);
    }
}
