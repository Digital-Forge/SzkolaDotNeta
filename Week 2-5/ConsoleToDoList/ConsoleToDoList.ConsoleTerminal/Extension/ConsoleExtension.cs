using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.ConsoleTerminal
{
    public static class ConsoleExtension
    {
        public static string ReadLine(string prefixText, string defoultText = "")
        {
            string pass = defoultText;
            ConsoleKey key;

            Console.Write(prefixText);

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }
    }
}
