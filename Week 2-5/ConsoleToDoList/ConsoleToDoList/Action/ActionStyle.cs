using ConsoleToDoList.ConsoleTerminal;

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
