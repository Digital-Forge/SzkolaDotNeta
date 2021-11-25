using System;

namespace ConsoleToDoList.ConsoleTerminal.Modul
{
    public static class ConsoleConfirmAlert
    {
        public class ConfirmAlertStyle
        {
            public ConsoleColorString ConfirmButton = new ConsoleColorString("Yes", ConsoleColor.Green);
            public ConsoleColorString SelectConfirmButton = new ConsoleColorString("Yes", ConsoleColor.Green, ConsoleColor.White);

            public ConsoleColorString NoConfirmButton = new ConsoleColorString("No", ConsoleColor.Red);
            public ConsoleColorString SelectNoConfirmButton = new ConsoleColorString("No", ConsoleColor.Red, ConsoleColor.White);

            public Orientation OrientationButton = ConfirmAlertStyle.Orientation.Horizontally;
            
            public enum Orientation
            {
                Horizontally,
                Vertically
            }
        }

        public static bool Show(ConsoleColorString alert, ConfirmAlertStyle style = null)
        {
            bool state = true;
            bool confirmState = false;

            if (style == null) style = new ConfirmAlertStyle();

            do
            {
                Console.Clear();

                alert.ConsoleWriteLine();

                if (state)
                {
                    switch (style.OrientationButton)
                    {
                        case ConfirmAlertStyle.Orientation.Horizontally:
                            style.SelectConfirmButton.ConsoleWrite();
                            Console.Write("    ");
                            style.NoConfirmButton.ConsoleWriteLine();
                            break;
                        case ConfirmAlertStyle.Orientation.Vertically:
                            style.SelectConfirmButton.ConsoleWriteLine();
                            style.NoConfirmButton.ConsoleWriteLine();
                            break;
                    }
                }
                else
                {
                    switch (style.OrientationButton)
                    {
                        case ConfirmAlertStyle.Orientation.Horizontally:
                            style.ConfirmButton.ConsoleWrite();
                            Console.Write("    ");
                            style.SelectNoConfirmButton.ConsoleWriteLine();
                            break;
                        case ConfirmAlertStyle.Orientation.Vertically:
                            style.ConfirmButton.ConsoleWriteLine();
                            style.SelectNoConfirmButton.ConsoleWriteLine();
                            break;
                    }
                }

                switch (style.OrientationButton)
                {
                    case ConfirmAlertStyle.Orientation.Horizontally:
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                confirmState = true;
                                break;
                            case ConsoleKey.LeftArrow:
                                state = true;
                                break;
                            case ConsoleKey.RightArrow:
                                state = false;
                                break;
                        }
                        break;
                    case ConfirmAlertStyle.Orientation.Vertically:
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                confirmState = true;
                                break;
                            case ConsoleKey.UpArrow:
                                state = true;
                                break;
                            case ConsoleKey.DownArrow:
                                state = false;
                                break;
                        }
                        break;
                }

                
            } while (!confirmState);

            return state;
        }
}
}
