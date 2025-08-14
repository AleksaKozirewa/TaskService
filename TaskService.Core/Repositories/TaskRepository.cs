using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskService.Core.Data;
using TaskService.Core.Entities;
using TaskService.Core.Interfaces;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.Core.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(AppDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaskEntity> AddAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(int pageNumber, int pageSize, TaskStatus? status)
        {
            IQueryable<TaskEntity> query = _context.Tasks.AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkOverdueTasksAsync()
        {
            var now = DateTime.UtcNow;
            var overdueTasks = await _context.Tasks
                .Where(t => t.DueDate < now && t.Status != TaskStatus.Completed)
                .ToListAsync();

            foreach (var task in overdueTasks)
            {
                task.Status = TaskStatus.Overdue;
                task.UpdatedAt = now;
                _logger.LogInformation("Задача ID {TaskId} помечена как просроченная.", task.Id);
            }

            await _context.SaveChangesAsync();
        }
    }
}
