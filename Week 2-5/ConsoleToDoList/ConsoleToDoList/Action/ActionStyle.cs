using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList
{
    partial class Action
    {
        ConsoleMenu.MenuStyle MainMenuStyle = new ConsoleMenu.MenuStyle();

        private void initStyle()
        {
            menuStyle();
        }

        private void menuStyle()
        {
            MainMenuStyle.TextPreFix = new ConsoleColorString("    ");
        }
    }
}
