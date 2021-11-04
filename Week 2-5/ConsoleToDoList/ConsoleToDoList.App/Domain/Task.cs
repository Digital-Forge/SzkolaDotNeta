using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    [Serializable]
    class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public List<Tag> _tagList = new List<Tag>();
        public DateTime? Date = null;
        public bool? FinishStatus = null;
    }
}
