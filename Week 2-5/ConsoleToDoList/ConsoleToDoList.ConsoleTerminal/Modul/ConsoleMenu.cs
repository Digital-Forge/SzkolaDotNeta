using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.ConsoleTerminal
{
    public class ConsoleMenu
    {
        public delegate void voidFunctionDelegate();

        public class MenuStyle
        {
            public ConsoleColor? TextColorOfSelectedOptionText = ConsoleColor.White;
            public ConsoleColor? TextBackgroundColorOfSelectedOptionText = ConsoleColor.DarkBlue;

            public ConsoleColorString TextPreFixOfSelectOption = new ConsoleColorString(" --> ");
            public ConsoleColorString TextPostFixOfSelectOption = new ConsoleColorString(" <--");

            public ConsoleColorString TextPreFix = new ConsoleColorString(" ");
            public ConsoleColorString TextPostFix = new ConsoleColorString("");


            public MenuStyle Clone()
            {
                return (MenuStyle)this.MemberwiseClone();
            }
        }

        private class rowOption
        {
            public ConsoleColorString text;
            public voidFunctionDelegate function;

            public rowOption(ConsoleColorString text, voidFunctionDelegate function)
            {
                this.text = text;
                this.function = function;
            }
        }

        public ConsoleColorString MenuTitle = null;
        public ConsoleKey? AutoBackKeyButton = null;

        private List<rowOption> rowOptionsList = new List<rowOption>();
        private MenuStyle style;
        private bool exit = false;
        private voidFunctionDelegate exitFun;
        

        public ConsoleMenu(MenuStyle style = null)
        {
            this.style = style == null ? new MenuStyle() : style;
            exitFun = this.exitFunction;
        }

        public void add(ConsoleColorString text, voidFunctionDelegate function)
        {
            rowOptionsList.Add(new rowOption(text, function));
        }

        public void show()
        {
            if (rowOptionsList.Count == 0) return;

            int position = 0;
            ConsoleColorString saveBufror;
            exit = false;

            do
            {
                Console.Clear();

                if (MenuTitle != null)    MenuTitle.ConsoleWriteLine();

                //paint select option
                saveBufror = rowOptionsList[position].text.Copy();

                foreach (var txt in rowOptionsList[position].text.Text)
                {
                    txt.color = style.TextColorOfSelectedOptionText;
                    txt.backgroundColor = style.TextBackgroundColorOfSelectedOptionText;
                }

                rowOptionsList[position].text = 
                    style.TextPreFixOfSelectOption + 
                    rowOptionsList[position].text + 
                    style.TextPostFixOfSelectOption;

                for (int i = 0; i < rowOptionsList.Count; i++)
                {
                    if (i == position)
                    {
                        rowOptionsList[i].text.ConsoleWriteLine();
                    }
                    else
                    {
                        (style.TextPreFix + rowOptionsList[i].text + style.TextPostFix).ConsoleWriteLine();
                    }
                }

                rowOptionsList[position].text = saveBufror;

                ConsoleKey action = Console.ReadKey().Key;           

                switch (action)
                {
                    case ConsoleKey.Enter:
                        if (rowOptionsList[position].function != null)  rowOptionsList[position].function();
                        break;
                    case ConsoleKey.UpArrow:
                        position = position - 1 >= 0 ? position - 1 : 0;
                        break;
                    case ConsoleKey.DownArrow:
                        position = position + 1 < rowOptionsList.Count ? position + 1 : position;
                        break;
                    default:
                        if (AutoBackKeyButton != null && action == AutoBackKeyButton) exitFunction();
                        break;
                }

            } while (!exit);
            exit = false;
        }

        public void exitFunction()
        {
            exit = true;
        }
    }
}
