using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;
using System;

namespace ConsoleToDoList
{
    partial class Action
    {
        private void ReportsMenu()
        {
            ConsoleMenu menu = new ConsoleMenu(MainMenuStyle);
            menu.AutoBackKeyButton = ConsoleKey.Backspace;
            menu.MenuTitle = new ConsoleColorString("Report Patterns");
            menu.add(new ConsoleColorString("Back"), menu.exitFunction);
            menu.add(new ConsoleColorString("Base"), ReportsBuilder.BaseReport);
            menu.add(new ConsoleColorString("Tag"), ReportsBuilder.TagReport);
            menu.show();
        }
    }
}
