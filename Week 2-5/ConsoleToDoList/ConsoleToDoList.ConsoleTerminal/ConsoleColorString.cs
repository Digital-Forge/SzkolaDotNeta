using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.ConsoleTerminal
{
    class ConsoleColorString
    {
        public struct colorText
        {
            public string text;
            public ConsoleColor? color;
        }

        private List<colorText> _text;
        private ConsoleColor _defoultColor;

        public void AddText(string text)
        {
            _text.Add(new colorText()
            {
                text = text,
                color = null
            });
        }

        public void AddText(string text, ConsoleColor color)
        {
            _text.Add(new colorText()
            {
                text = text,
                color = color
            });
        }

        public void ConsoleWrite()
        {
            _defoultColor = Console.ForegroundColor;

            for (int i = 0; i < _text.Count; i++)
            {
                Console.ForegroundColor = _text[i].color != null ? (ConsoleColor)_text[i].color : _defoultColor;
                Console.Write(_text[i].text);
            }

            Console.ForegroundColor = _defoultColor;
        }

        public void ConsoleWriteLine()
        {
            ConsoleWrite();
            Console.WriteLine();
        }

        public void Clear()
        {
            _text.Clear();
        }
    }
}
