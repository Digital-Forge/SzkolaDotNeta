using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    public interface ITask : IConsoleReadable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
