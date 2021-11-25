using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleToDoList
{
    partial class Action
    {
        private List<TaskHook> list;

        private void ViewTasks()
        {
            bool reload;
            list = LogicCORE.Core.TasksData.NextNodes != null ?
                LogicCORE.Core.TasksData.NextNodes.Select(x => x.Data as TaskHook).ToList() :
                new List<TaskHook>();
            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.add(new ConsoleColorString("Serch"), () => { VT_SerchMenu(); menu.exitFunction(); reload = true; });
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
            menu.MenuTitle = new ConsoleColorString("Serch by", ConsoleColor.Magenta);
            menu.add(new ConsoleColorString("Name"), null);
            menu.add(new ConsoleColorString("Tag"), null);
            menu.add(new ConsoleColorString("Status"), null);
            menu.add(new ConsoleColorString("Data"), null);
            menu.add(new ConsoleColorString("Priority"), null);
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.show();
        }

        private void VT_SortByMenu()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.MenuTitle = new ConsoleColorString("Sort by", ConsoleColor.Magenta);
            menu.add(new ConsoleColorString("Name A-Z"), null);
            menu.add(new ConsoleColorString("Name Z-A"), null);
            menu.add(new ConsoleColorString("Data"), null);
            menu.add(new ConsoleColorString("Priority"), null);
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.show();
        }
    }
}