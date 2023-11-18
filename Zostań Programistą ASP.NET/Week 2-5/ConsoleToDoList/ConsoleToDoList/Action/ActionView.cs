using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleToDoList
{
    partial class Action
    {
        private List<TaskHook> list;

        private void ViewTasks()
        {
            bool reload;
            VT_SerchMenuClean();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.add(new ConsoleColorString("Search"), () => { VT_SerchMenu(); menu.exitFunction(); reload = true; });
                menu.add(new ConsoleColorString("SortBy"), () => { VT_SortByMenu(); menu.exitFunction(); reload = true; });
                menu.add(new ConsoleColorString("Back"), menu.exitFunction);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                foreach (var item in list)
                {
                    menu.add(item.RecordHeader(), () => { item.View(); menu.exitFunction(); reload = true; checkIsDelete(item); });
                }
                menu.show();
            } while (reload);
        }

        private void checkIsDelete(TaskHook hook)
        {
            if (hook.Node == null)
            {
                list.Remove(hook);
            }
        }

        private void VT_SerchMenu()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.MenuTitle = new ConsoleColorString("Search by", ConsoleColor.Magenta);
            menu.add(new ConsoleColorString("Name"), () => { VT_SerchMenuByName(); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Tag"), () => { VT_SerchMenuByTag(); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Status"), () => { VT_SerchMenuByStatus(); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Data"), () => { VT_SerchMenuByDate(); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Priority"), () => { VT_SerchMenuByPriority(); menu.exitFunction(); });
            menu.add(new ConsoleColorString("Clean"), () => { VT_SerchMenuClean(); menu.exitFunction(); });
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.show();
        }

        private void VT_SortByMenu()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.MenuTitle = new ConsoleColorString("Sort by", ConsoleColor.Magenta);
            menu.add(new ConsoleColorString("Name A-Z"), () =>
            {
                list = list.OrderBy(x => x.Task.Name).ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Data"), () =>
            {
                list = list.Where(x => drowMinData(x) != null).
                            OrderBy(x => drowMinData(x).GetValueOrDefault()).
                Concat(list.Where(x => drowMinData(x) == null)).
                ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Priority"), () =>
            {
                list = list.Where(x => x.Task.Priority == TaskPriority.Hight).
                Concat(list.Where(x => x.Task.Priority == TaskPriority.Meddium).
                Concat(list.Where(x => x.Task.Priority == TaskPriority.Low).
                Concat(list.Where(x => x.Task.Priority == TaskPriority.NoPriority)))).ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Reverse"), () =>
            {
                list.Reverse();
                menu.exitFunction();
            });
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.show();
        }

        private DateTime? drowMinData(TaskHook hook)
        {
            if (hook != null)
            {
                var date = Node.NormalizationNodeToList(hook.Node).
                    Where(x => (x.Data as TaskHook)?.Task?.Date != null).
                    OrderBy((x => (x.Data as TaskHook).Task.Date.Value)).
                    ToList();

                return date.Count != 0 
                    ? (date[0].Data as TaskHook).Task.Date 
                    : null;
            }
            return null;
        }

        private DateTime? drowMaxData(TaskHook hook)
        {
            if (hook != null)
            {
                var date = Node.NormalizationNodeToList(hook.Node).
                    Where(x => (x.Data as TaskHook)?.Task?.Date != null).
                    OrderBy((x => (x.Data as TaskHook).Task.Date.Value)).
                    ToList();

                return date.Count != 0
                    ? (date[date.Count - 1].Data as TaskHook).Task.Date
                    : null;
            }
            return null;
        }

        private List<DateTime> drowData(TaskHook hook)
        {
            if (hook != null)
            {
                var date = Node.NormalizationNodeToList(hook.Node).
                    Where(x => (x.Data as TaskHook)?.Task?.Date != null).
                    Select((x => (x.Data as TaskHook).Task.Date.Value)).
                    ToList();

                return date;
            }
            return null;
        }

        private void VT_SerchMenuByName()
        {
            Console.Clear();
            Console.Write("Task Name : ");
            string buff = Console.ReadLine();

            if (!string.IsNullOrEmpty(buff))
            {
                Regex regex = new Regex($"{buff}");
                list = list.Where(x => regex.IsMatch(x.Task.Name)).OrderBy(x => x.Task.Name).ToList();
            }
        }

        private void VT_SerchMenuByTag()
        {
            bool reload;
            List<Tag> tagList = LogicCORE.Core.TagsData.TagsList.OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
                menu.add(new ConsoleColorString("Search"), () => 
                {
                    Console.Clear();
                    Console.Write("Tag Name : ");
                    string buff = Console.ReadLine();

                    if (!string.IsNullOrEmpty(buff))
                    {
                        Regex regex = new Regex($"{buff}");
                        tagList = tagList.Where(x => regex.IsMatch(x.TagName)).OrderBy(x => x.TagName).ToList();
                    }
                    reload = true; 
                    menu.exitFunction(); 
                });

                foreach (var item in tagList)
                {
                    menu.add(new ConsoleColorString(item.TagName, ConsoleColor.Yellow), () =>
                    {
                        list = list.Where(x => x.TagsBag.TagsList.Exists(y => y.TagName.ToLower() == item.TagName.ToLower())).ToList();
                        menu.exitFunction();
                    });
                }
                menu.show();
            } while (reload);
        }

        private void VT_SerchMenuByPriority()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.add(new ConsoleColorString("NoPriority"), () =>
            {
                list = list.Where(x => x.Task.Priority == TaskPriority.NoPriority).ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Hight", ConsoleColor.Red), () =>
            {
                list = list.Where(x => x.Task.Priority == TaskPriority.Hight).ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Meddium", ConsoleColor.Yellow), () =>
            {
                list = list.Where(x => x.Task.Priority == TaskPriority.Meddium).ToList();
                menu.exitFunction();
            });
            menu.add(new ConsoleColorString("Low", ConsoleColor.Green), () =>
            {
                list = list.Where(x => x.Task.Priority == TaskPriority.Low).ToList();
                menu.exitFunction();
            });
            menu.show();
        }

        private void VT_SerchMenuByDate()
        {
            ConsoleDataReader input = new ConsoleDataReader(new DateAdapter(), DataReaderStyle);
            input.DataRead();

            var date = input.GetReadObject as DateAdapter;

            if (date.Date != null)
            {
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.add(new ConsoleColorString($" > {date.Date.Value.ToString("d MMM yyyy")}"), () =>
                {
                    list = list.Where(x => drowMinData(x) != null).
                                Where(x => drowMinData(x).Value > date.Date.Value 
                                        || drowMaxData(x).Value > date.Date.Value).ToList();
                    menu.exitFunction();
                });
                menu.add(new ConsoleColorString($" >= {date.Date.Value.ToString("d MMM yyyy")}"), () =>
                {
                    list = list.Where(x => drowMinData(x) != null).
                                Where(x => drowMinData(x).Value >= date.Date.Value
                                        || drowMaxData(x).Value >= date.Date.Value).ToList();
                    menu.exitFunction();
                });
                menu.add(new ConsoleColorString($" < {date.Date.Value.ToString("d MMM yyyy")}"), () =>
                {
                    list = list.Where(x => drowMinData(x) != null).
                                Where(x => drowMinData(x).Value < date.Date.Value
                                        || drowMaxData(x).Value < date.Date.Value).ToList();
                    menu.exitFunction();
                });
                menu.add(new ConsoleColorString($" <= {date.Date.Value.ToString("d MMM yyyy")}"), () =>
                {
                    list = list.Where(x => drowMinData(x) != null).
                                Where(x => drowMinData(x).Value <= date.Date.Value
                                        || drowMaxData(x).Value <= date.Date.Value).ToList();
                    menu.exitFunction();
                });
                menu.add(new ConsoleColorString($" = {date.Date.Value.ToString("d MMM yyyy")}"), () =>
                {
                    list = list.Where(x => drowData(x).Exists(y => y.Year == date.Year && y.Month == date.Month && y.Day == date.Day)).ToList();
                    menu.exitFunction();
                });
                menu.show();
            }
        }

        private void VT_SerchMenuByStatus()
        {
            var style = new ConsoleConfirmAlert.ConfirmAlertStyle();
            style.ConfirmButton = new ConsoleColorString("To do");
            style.SelectConfirmButton = new ConsoleColorString("To do", null, ConsoleColor.White);
            style.NoConfirmButton = new ConsoleColorString("Finish");
            style.SelectNoConfirmButton = new ConsoleColorString("Finish", null, ConsoleColor.White);
            style.OrientationButton = ConsoleConfirmAlert.ConfirmAlertStyle.Orientation.Vertically;
            if (ConsoleConfirmAlert.Show("",null,null, style))
            {
                list = list.Where(x => x.FinishStatus == false).ToList();
            }
            else
            {
                list = list.Where(x => x.FinishStatus == true).ToList();
            }
        }

        private void VT_SerchMenuClean()
        {
            list = LogicCORE.Core.TasksData.NextNodes != null ?
                LogicCORE.Core.TasksData.NextNodes.Select(x => x.Data as TaskHook).OrderBy(x => x.FinishStatus).ToList() :
                new List<TaskHook>();
        }
    }
}