using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList
{
    partial class Action
    {
        private void MainMenu()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);

            menu.MenuTitle = new ConsoleColorString("      TO ", ConsoleColor.Red).AddText("DO ", ConsoleColor.Yellow).AddText("LIST ", ConsoleColor.Green);

            menu.add(new ConsoleColorString("View"), ViewTasks);
            menu.add(new ConsoleColorString("Add new Task"), AddNewTask);
            menu.add(new ConsoleColorString("Raport"), null);
            menu.add(new ConsoleColorString("Options"), Options);
            menu.add(new ConsoleColorString("Exit"), menu.exitFunction);

            menu.show();
        }
    }
}
