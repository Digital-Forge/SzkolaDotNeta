using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleToDoList.ConsoleTerminal
{
    public class ConsoleColorString
    {
        public class colorText
        {
            public string text;
            public ConsoleColor? color;
            public ConsoleColor? backgroundColor;

            public colorText(string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
            {
                this.text = text;
                this.color = color;
                this.backgroundColor = backgroundColor;
            }
        }

        private List<colorText> _text = new List<colorText>();


        public List<colorText> TextElementsList { get => _text; }

        public string Text 
        {
            get
            {
                string buff = "";

                foreach (var txt in _text)
                {
                    buff += txt.text;
                }

                return buff;
            }
        }

        public ConsoleColorString()
        {

        }

        public ConsoleColorString(string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
        {
            AddText(text, color, backgroundColor);
        }

        public ConsoleColorString AddText(string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
        {
            _text.Add(new colorText(text, color, backgroundColor));
            return this;
        }

        public void ConsoleWrite()
        {
            ConsoleColor[] backupConsolColor = new ConsoleColor[2];

            backupConsolColor[0] = Console.ForegroundColor;
            backupConsolColor[1] = Console.BackgroundColor;

            for (int i = 0; i < _text.Count; i++)
            {
                Console.ForegroundColor = _text[i].color != null ? (ConsoleColor)_text[i].color : backupConsolColor[0];
                Console.BackgroundColor = _text[i].backgroundColor != null ? (ConsoleColor)_text[i].backgroundColor : backupConsolColor[1];
                Console.Write(_text[i].text);
            }

            Console.ForegroundColor = backupConsolColor[0];
            Console.BackgroundColor = backupConsolColor[1];
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

        public ConsoleColorString Copy()
        {
            ConsoleColorString buff = new ConsoleColorString();
            var buffCell = buff._text;

            this._text.ForEach(x => buffCell.Add(new colorText(new string(x.text), x.color, x.backgroundColor)));

            return buff;
        }

        public static ConsoleColorString operator + (ConsoleColorString a, ConsoleColorString b)
        {
            return new ConsoleColorString() { _text = new List<colorText>().Concat(a._text).Concat(b._text).ToList()};
        }

        public int Length()
        {
            if (_text.Count == 0) return 0;

            int length = 0;

            foreach (var item in _text)
            {
                length += item.text.Length;
            }

            return length;
        }  
    }
}