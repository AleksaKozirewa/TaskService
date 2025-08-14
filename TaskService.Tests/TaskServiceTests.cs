//using Moq;
//using TaskService.Core.Entities;
//using TaskService.Core.Interfaces;
//using TaskService.Core.Services;
//using TaskStatus = TaskService.Core.Entities.TaskStatus;

//namespace TaskService.Tests
//{
//    public class TaskServiceTests
//    {
//        private readonly Mock<ITaskRepository> _mockRepo;
//        private readonly ITaskService _taskService;

//        public TaskServiceTests()
//        {
//            _mockRepo = new Mock<ITaskRepository>();
//            _taskService = new TasksService(_mockRepo.Object);
//        }

//        [Fact]
//        public async Task CreateTask_ShouldReturnTask_WhenValidInput()
//        {
//            // Arrange
//            var title = "Test Task";
//            var description = "Test Description";
//            var dueDate = DateTime.Now.AddDays(1);
//            var expectedTask = new TaskEntity
//            {
//                Id = Guid.NewGuid(),
//                Title = title,
//                Description = description,
//                DueDate = dueDate,
//                Status = TaskStatus.New
//            };

//            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TaskEntity>()))
//                .ReturnsAsync(expectedTask);

//            // Act
//            var result = await _taskService.CreateTaskAsync(title, description, dueDate);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(title, result.Title);
//            Assert.Equal(description, result.Description);
//            Assert.Equal(dueDate, result.DueDate);
//            Assert.Equal(TaskStatus.New, result.Status);
//        }

//        [Fact]
//        public async Task UpdateTask_ShouldThrowException_WhenTaskNotFound()
//        {
//            // Arrange
//            var taskId = Guid.NewGuid();
//            _mockRepo.Setup(repo => repo.GetByIdAsync(taskId))
//                .ReturnsAsync((TaskEntity)null);

//            // Act & Assert
//            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
//                _taskService.UpdateTaskAsync(taskId, "New Title", "New Desc", DateTime.Now, TaskStatus.InProgress));
//        }
//    }
//}