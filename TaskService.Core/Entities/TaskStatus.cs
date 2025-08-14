using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Core.Entities
{
    public enum TaskStatus
    {
        New,        // Задача создана, но не начата
        InProgress, // Задача в работе
        Completed,  // Задача завершена успешно
        Overdue     // Задача просрочена
    }
}
