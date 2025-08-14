using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Core.Entities;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskEntity> AddAsync(TaskEntity task);
        Task<TaskEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskEntity>> GetAllAsync(int pageNumber, int pageSize, TaskStatus? status);
        Task UpdateAsync(TaskEntity task);
        Task DeleteAsync(Guid id);
        Task MarkOverdueTasksAsync();
    }
}
