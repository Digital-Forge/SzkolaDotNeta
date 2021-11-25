using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;

namespace ConsoleToDoList
{
    partial class Action
    {
        ConsoleMenu.MenuStyle MainMenuStyle = new ConsoleMenu.MenuStyle();
        ConsoleMenu.MenuStyle ViewMenuStyle = new ConsoleMenu.MenuStyle();
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

            ViewMenuStyle.TextPreFix = new ConsoleColorString("    ");
            ViewMenuStyle.TextPostFixOfSelectOption = new ConsoleColorString();
        }

        private void readerStyle()
        {
            DataReaderStyle.TextMiddleFixOfSelectItem = new ConsoleColorString(" = ");
            DataReaderStyle.ValueColor = System.ConsoleColor.Green;
            DataReaderStyle.ValueColorOfSelectedItem = System.ConsoleColor.Red;
        }

        private void addStyles()
        {
            TaskBuilder.DataConsoleReaderStyle = DataReaderStyle;
            TaskBuilder.MenuStyle = MainMenuStyle;
            TaskHook.MenuStyle = MainMenuStyle;
            TaskHook.DataReaderStyle = DataReaderStyle;
        }
    }
}
