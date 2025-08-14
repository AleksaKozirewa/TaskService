namespace TaskService.Core.Entities
{
    /// <summary>
    /// Статусы выполнения задачи.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// Задача создана, но ещё не начата.
        /// </summary>
        New,

        /// <summary>
        /// Задача находится в процессе выполнения.
        /// </summary>
        InProgress,

        /// <summary>
        /// Задача успешно завершена.
        /// </summary>
        Completed,

        /// <summary>
        /// Задача не была выполнена в установленный срок.
        /// </summary>
        Overdue
    }
}
