using ConsoleToDoList.ConsoleTerminal;
using System;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class TaskHook : INodeDataIntegration
    {
        public Node Node { get; set; } = null;

        public Task Task = new Task();

        public TagsBag TagsBag = new TagsBag(); 

        public DateTime? Date
        {
            get => Task.Date;
            set
            {
                if (Task.FinishStatus == false) Task.Date = value;
            }
        }

        public bool FinishStatus
        {
            get => Task.FinishStatus;
            set
            {
                Task.FinishStatus = value;
                if (Task.FinishStatus) Task.Date = DateTime.Now;
            }
        }

        public void AddTask()
        {
            if (Node == null) Node = new Node();
            new TaskBuilder().Build(Node);
        }

        public void ModifyData()
        {
            new TaskBuilder().Build(Node, true);
        }

        public void ModifyDate()
        {
            
        }

        public void View()
        {

        }

        public void ModifyTag()
        {

        }

        public ConsoleColorString Header()
        {
            ConsoleColorString buff = new ConsoleColorString(" Title : ").AddText($"{Task.Name}\n", ConsoleColor.Green);
            _CCS_PriorityDate(buff, Task);
            buff.AddText("\n Description : ").AddText($"{Task.Description}\n", ConsoleColor.Yellow).
                AddText(" Tags :");

            if (TagsBag.cellsOfTagsList.Count != 0) buff.AddText("\n");

            foreach (var item in TagsBag.cellsOfTagsList)
            {
                if (item.Lock) buff.AddText($"  {item.Tag.TagName}", ConsoleColor.Gray);
                else           buff.AddText($"  {item.Tag.TagName}", ConsoleColor.White);
            }

            buff.AddText("\n SubTasks : \n");
            if (Node != null)
            {
                if (Node.NextNodes != null)
                {
                    foreach (var item in Node.NextNodes)
                    {
                        buff.AddText("     -> ").AddText(((TaskHook)(item.Data)).Task.Name, ConsoleColor.Green).AddText("\n          ");
                        _CCS_PriorityDate(buff, ((TaskHook)(item.Data)).Task).AddText("\n");
                    }
                }
            }
            return buff;
        }

        public ConsoleColorString RecordHeader()
        {
            ConsoleColorString buff = new ConsoleColorString($"{Task.Name}\n", ConsoleColor.Green).AddText("   Priority : ");
            _CCS_PriorityDate(buff, Task);
            return buff;
        }

        private ConsoleColorString _CCS_PriorityDate(ConsoleColorString buff, Task task)
        {
            buff.AddText(" Priority : ");
            switch (task.Priority)
            {
                case TaskPriority.NoPriority:
                    buff.AddText(" - - - ", ConsoleColor.White);
                    break;
                case TaskPriority.Hight:
                    buff.AddText("Hight", ConsoleColor.Red);
                    break;
                case TaskPriority.Meddium:
                    buff.AddText("Meddium", ConsoleColor.Yellow);
                    break;
                case TaskPriority.Low:
                    buff.AddText("Low", ConsoleColor.Green);
                    break;
            }

            if (task.Date != null)
            {
                if (task.FinishStatus == true) buff.AddText("    Date : ").AddText($"{task.Date.ToString()}");
                else
                {
                    if (task.Date > DateTime.Now)                                               buff.AddText("    Termin : ").AddText($"{task.Date.ToString()}", ConsoleColor.DarkRed);
                    else if (task.Date <= DateTime.Now && task.Date > DateTime.Now.AddDays(-7)) buff.AddText("    Termin : ").AddText($"{task.Date.ToString()}", ConsoleColor.Red);
                    else                                                                        buff.AddText("    Termin : ").AddText($"{task.Date.ToString()}", ConsoleColor.Yellow);
                }
            }

            return buff;
        }

    }
}
