using ConsoleToDoList.App.System;
using ConsoleToDoList.ConsoleTerminal;

namespace ConsoleToDoList
{
    partial class Action
    {
        ConsoleMenu.MenuStyle MainMenuStyle = new ConsoleMenu.MenuStyle();
        IConsoleDataReader.DataConsoleReaderStyle DataReaderStyle = new IConsoleDataReader.DataConsoleReaderStyle();

        private void initStyle()
        {
            menuStyle();
            readerStyle();
            addStyles();
        }

        private void menuStyle()
        {
            MainMenuStyle.TextPreFix = new ConsoleColorString("    ");
        }

        private void readerStyle()
        {

        }

        private void addStyles()
        {
            TaskBuilder.DataConsoleReaderStyle = DataReaderStyle;
            TaskBuilder.MenuStyle = MainMenuStyle;
        }
    }
}
