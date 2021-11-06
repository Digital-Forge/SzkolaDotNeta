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
        public List<Tag> TagList = new List<Tag>();
        //public DateTime? Date = null;
        //public bool? FinishStatus = null;

        private DateTime? _date = null;
        private bool? _finishStatus;

        public DateTime? Date
        {
            get => _date;
            set 
            {
                if (_finishStatus == null) _date = value;
            }
        }

        public bool? FinishStatus
        {
            get => _finishStatus;
            set
            {
                switch (value)
                {
                    case null:
                        break;
                    case true:
                        _date = DateTime.Now;
                        break;
                    case false:
                        _date = DateTime.Now;
                        break;
                }
                _finishStatus = value;
            }
        }


    }
}
