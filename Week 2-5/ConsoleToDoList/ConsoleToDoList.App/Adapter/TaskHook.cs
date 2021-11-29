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

        public static ConsoleMenu.MenuStyle MenuStyle = null;
        public static IConsoleDataReader.DataConsoleReaderStyle DataReaderStyle = null;

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
                if (Task.FinishStatus)
                {
                    if (Node != null)
                    {
                        if (Node.NextNodes != null)
                        {
                            foreach (var item in Node.NextNodes)
                            {
                                (item.Data as TaskHook).FinishStatus = Task.FinishStatus;
                            }
                        }
                    }
                    Task.Date = DateTime.Now;
                }
                LogicCORE.Save();
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
            Action<DateTime?> func = (date) =>
            {
                ConsoleDataReader reader = new ConsoleDataReader(new DateAdapter(date), DataReaderStyle);
                reader.DataRead();

                if ((reader.GetReadObject as DateAdapter).Date != null)
                {
                    if ((reader.GetReadObject as DateAdapter).Date < DateTime.Now)
                    {
                        if(!ConsoleConfirmAlert.Show("The date has expired. Do you want to save anyway?"))
                        {
                            return;
                        }
                    }
                    Task.Date = (reader.GetReadObject as DateAdapter).Date;
                }
            };

            if (Task.Date == null)
            {
                func(null);
            }
            else
            {
                var menu = new ConsoleMenu(MenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;
                menu.add(new ConsoleColorString("Set Date"), () => { func(new DateTime(Task.Date.Value.Year, Task.Date.Value.Month, Task.Date.Value.Day)); menu.exitFunction(); });
                menu.add(new ConsoleColorString("Delete"), () => { Task.Date = null; menu.exitFunction(); });
                menu.add(new ConsoleColorString("Back"), menu.exitFunction);
                menu.show();
            }
        }

        public void ModifyTag()
        {
            var menu = new ConsoleMenu(MenuStyle);
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.add(new ConsoleColorString("Add"), () => { TagController.AddTags(this); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Remove"), () => { TagController.RemoveTags(this); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Back"), menu.exitFunction);
            menu.show();
        }

        public void DeleteTask()
        {
            Node?.RemoveThisNode();
            LogicCORE.Save();
        }

        public void View()
        {
            ConsoleMenu menu;
            bool reload;

            do
            {
                reload = false;
                menu = new ConsoleMenu(MenuStyle);
                menu.MenuTitle = Header(false).AddText("\n SubTasks : \n");

                if (Node != null)
                {
                    if (Node.NextNodes != null)
                    {
                        foreach (var item in Node.NextNodes)
                        {
                            menu.add(new ConsoleColorString("      ") + (((TaskHook)(item.Data)).RecordHeader()), () => { reload = true; menu.exitFunction(); ((TaskHook)(item.Data)).View(); });
                        }
                    }
                }

                if (!FinishStatus)
                {
                    menu.add(new ConsoleColorString("Add New Subtask"), () => { AddTask(); reload = true; menu.exitFunction(); });
                    menu.add(new ConsoleColorString("Edit"), () => { ModifyData(); reload = true; menu.exitFunction(); });
                    menu.add(new ConsoleColorString("Check"), () => { FinishStatus = true; menu.exitFunction(); });
                }
                menu.add(new ConsoleColorString("Delete"), () => { DeleteTask(); menu.exitFunction(); });
                menu.show();
            } while (reload); 
        }

        public ConsoleColorString Header(bool addSubtask = true)
        {
            ConsoleColorString buff = new ConsoleColorString();
            _CCS_StatusTask(buff);
            buff.AddText(" Title : ").AddText($"{Task.Name}\n", ConsoleColor.Green);
            _CCS_PriorityDate(buff);
            buff.AddText("\n Description : ").AddText($"{Task.Description}\n", ConsoleColor.Yellow).
                AddText(" Tags :");

            if (TagsBag.cellsOfTagsList.Count != 0) buff.AddText("\n");

            foreach (var item in TagsBag.cellsOfTagsList)
            {
                if (item.Lock) buff.AddText($"  {item.Tag.TagName}", ConsoleColor.Gray);
                else           buff.AddText($"  {item.Tag.TagName}", ConsoleColor.White);
            }

            if (addSubtask)
            {
                buff.AddText("\n SubTasks : \n");
                if (Node != null)
                {
                    if (Node.NextNodes != null)
                    {
                        foreach (var item in Node.NextNodes)
                        {
                            buff = buff.AddText("     -> ") + (((TaskHook)(item.Data)).RecordHeader());
                        }
                    }
                }
            }
            return buff;
        }

        public ConsoleColorString RecordHeader()
        {
            ConsoleColorString buff = new ConsoleColorString($"{Task.Name}\n          ", ConsoleColor.Green);
            _CCS_StatusTask(buff);
            _CCS_PriorityDate(buff);
            return buff.AddText("\n");
        }

        private ConsoleColorString _CCS_PriorityDate(ConsoleColorString buff)
        {
            buff.AddText(" Priority : ");
            switch (Task.Priority)
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

            if (Task.Date != null)
            {
                if (FinishStatus) buff.AddText("    Date : ").AddText($"{Task.Date.ToString()}");
                else
                {
                    if (Task.Date < DateTime.Now)                                               buff.AddText("    Termin : ").AddText($"{Task.Date.Value.ToString("d MMM yyyy")}", ConsoleColor.DarkRed);
                    else if (Task.Date >= DateTime.Now && Task.Date < DateTime.Now.AddDays(-7)) buff.AddText("    Termin : ").AddText($"{Task.Date.Value.ToString("d MMM yyyy")}", ConsoleColor.Red);
                    else                                                                        buff.AddText("    Termin : ").AddText($"{Task.Date.Value.ToString("d MMM yyyy")}", ConsoleColor.Yellow);
                }
            }
            return buff;
        }

        private void _CCS_StatusTask(ConsoleColorString txt)
        {
            if (Task.FinishStatus) txt.AddText(" Satus : ").AddText("Finish", ConsoleColor.Blue, ConsoleColor.White);
        }


    }
}