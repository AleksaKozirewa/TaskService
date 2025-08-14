using Moq;
using TaskService.Core.Entities;
using TaskService.Core.Interfaces;
using TaskService.Core.Services;
using TaskStatus = TaskService.Core.Entities.TaskStatus;

namespace TaskService.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TasksService _service;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _service = new TasksService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldCreateTask_WhenValidData()
        {
            // Arrange
            var testTitle = "Test Task";
            var testDescription = "Test Description";
            var testDueDate = DateTime.Now.AddDays(1);
            var expectedTask = new TaskEntity
            {
                Title = testTitle,
                Description = testDescription,
                DueDate = testDueDate.ToUniversalTime(),
                Status = TaskStatus.New
            };

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskEntity>()))
                          .ReturnsAsync(expectedTask);

            // Act
            var result = await _service.CreateTaskAsync(testTitle, testDescription, testDueDate);

            // Assert
            Assert.Equal(expectedTask.Title, result.Title);
            Assert.Equal(expectedTask.Description, result.Description);
            Assert.Equal(expectedTask.DueDate, result.DueDate);
            Assert.Equal(TaskStatus.New, result.Status);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var expectedTask = new TaskEntity { Id = taskId, Title = "Test Task" };

            _mockRepository.Setup(x => x.GetByIdAsync(taskId))
                          .ReturnsAsync(expectedTask);

            // Act
            var result = await _service.GetTaskByIdAsync(taskId);

            // Assert
            Assert.Equal(expectedTask.Id, result.Id);
            Assert.Equal(expectedTask.Title, result.Title);
            _mockRepository.Verify(x => x.GetByIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetByIdAsync(taskId))
                          .ReturnsAsync((TaskEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetTaskByIdAsync(taskId));
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepository.Setup(x => x.GetByIdAsync(taskId))
                          .ReturnsAsync((TaskEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateTaskAsync(taskId, "Title", "Desc", DateTime.Now, TaskStatus.New));
        }
    }
}