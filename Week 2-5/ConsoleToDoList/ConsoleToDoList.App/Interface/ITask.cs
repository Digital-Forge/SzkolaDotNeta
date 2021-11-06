using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    interface ITask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
