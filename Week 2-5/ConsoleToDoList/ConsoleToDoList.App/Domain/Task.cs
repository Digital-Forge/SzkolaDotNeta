using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class Task : ITask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? Date = null;
        public bool FinishStatus = false;
    }
}
