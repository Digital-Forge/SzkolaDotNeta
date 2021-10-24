using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    class Task
    {
        public enum TaskPriority
        {
            Hight,
            Meddium,
            Low,
            NoPriority
        }

        private List<Tag> _tagList = null;
        private TaskPriority _priority;
        private string _name;
        private string _description;

        public Task(string name, TaskPriority priority = TaskPriority.NoPriority)
        {
            _name = name;
            _priority = priority;
        }
    }
}
