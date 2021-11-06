using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleToDoList.ConsoleTerminal
{
    public class ConsoleDataReader : IConsoleDataReader
    {
        private enum propertySignature
        {
            _enum,
            _class,
            _default
        };

        private IConsoleReadable obj;

        public object GetReadObject { get => obj; }

        public Type GetReadObjectType { get => obj.GetType(); }
        public IConsoleDataReader.DataConsoleReaderStyle Style { get; set; }

        public ConsoleDataReader(IConsoleReadable obj, IConsoleDataReader.DataConsoleReaderStyle style = null)
        {
            this.obj = obj;
            this.Style = style != null ? style : new IConsoleDataReader.DataConsoleReaderStyle();
        }

        public void DataRead()
        {
            if (obj == null) return;

            int position = 0;
            bool exit = false;
            bool edit = false;
            
            var properties = obj.GetType().GetProperties();

            do
            {
                Console.Clear();

                if (edit)
                {
                    switch (PropertySignature(properties[position]))
                    {
                        case propertySignature._enum:
                            _editEnum(ref position, properties);
                            break;
                        case propertySignature._class:
                            _editClass(ref position, properties);
                            break;
                        case propertySignature._default:
                            view_editDefoult(ref position, ref exit, properties, ref edit);
                            break;
                    }
                    edit = false;
                }
                else
                {
                    view_editDefoult(ref position, ref exit, properties, ref edit);
                }                
            } while (!exit);
        }

        private propertySignature PropertySignature(System.Reflection.PropertyInfo property)
        {
            if (typeof(IConsoleReadable).IsAssignableFrom(property.PropertyType)) return propertySignature._class;
            else if (property.PropertyType.IsEnum) return propertySignature._enum;
            else return propertySignature._default;

            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void view_editDefoult(ref int position, ref bool exit, System.Reflection.PropertyInfo[] properties, ref bool edit)
        {
            ConsoleColorString lineBuff = new ConsoleColorString();

            for (int i = 0; i < properties.Length; i++)
            {
                lineBuff.Clear();

                if (i == position)
                {
                    lineBuff += Style.TextPrefixOfSelectItem;
                    lineBuff.AddText($"{properties[i].Name}", Style.DescriptionColorOfSelectedItem, Style.DescriptionBackgroundColorOfSelectedItem);
                    lineBuff += Style.TextMiddleFixOfSelectItem;
                    if (!edit)
                        switch (PropertySignature(properties[i]))
                        {
                            case propertySignature._enum:
                            case propertySignature._default:
                                lineBuff.AddText($"{properties[i].GetValue(this.obj)}", Style.ValueColorOfSelectedItem, Style.ValueBackgroundColorOfSelectedItem);
                                break;
                            case propertySignature._class:
                                lineBuff.AddText($"(...)", Style.ValueColorOfSelectedItem, Style.ValueBackgroundColorOfSelectedItem);
                                break;
                        }
                    
                }
                else
                {
                    lineBuff += Style.TextPrefix;
                    lineBuff.AddText($"{properties[i].Name}");
                    lineBuff += Style.TextMiddleFix;

                    switch (PropertySignature(properties[i]))
                    {
                        case propertySignature._enum:
                            lineBuff.AddText($"{properties[i].GetValue(this.obj).ToString()}", Style.ValueColor, Style.ValueBackgroundColor);
                            break;
                        case propertySignature._class:
                            lineBuff.AddText($"(...)", Style.ValueColor, Style.ValueBackgroundColor);

                            break;
                        case propertySignature._default:
                            lineBuff.AddText($"{properties[i].GetValue(this.obj)}", Style.ValueColor, Style.ValueBackgroundColor);
                            break;
                    }
                }
                lineBuff.ConsoleWriteLine();
            }
            Console.WriteLine();
            Style.ActionDescription.ConsoleWriteLine();

            if (edit)
            {
                Console.SetCursorPosition(
                    properties[position].Name.Length
                        + Style.TextPrefixOfSelectItem.Length()
                        + Style.TextMiddleFixOfSelectItem.Length(),
                    position);


                ConsoleColor[] buff = new ConsoleColor[2]
                    { 
                        Console.ForegroundColor,
                        Console.BackgroundColor
                    };
                    
                Console.ForegroundColor = Style.ValueColorOfSelectedItem == null ? Console.ForegroundColor : (ConsoleColor)Style.ValueColorOfSelectedItem;
                Console.BackgroundColor = Style.ValueBackgroundColorOfSelectedItem == null ? Console.BackgroundColor : (ConsoleColor)Style.ValueBackgroundColorOfSelectedItem;
                //string line = ReadLine.Read("", $"{properties[position].GetValue(this.obj).ToString()}");
                //string line = Console.ReadLine();
                string line = ConsoleExtension.ReadLine("", $"{properties[position].GetValue(this.obj).ToString()}");
                Console.ForegroundColor = buff[0];
                Console.BackgroundColor = buff[1];

                try
                {
                    properties[position].SetValue(this.obj, Convert.ChangeType(line, properties[position].PropertyType));
                }
                catch (Exception)
                {

                }
                edit = false;
            }
            else
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        edit = true;
                        break;
                    case ConsoleKey.UpArrow:
                        position = position - 1 >= 0 ? position - 1 : 0;
                        break;
                    case ConsoleKey.DownArrow:
                        position = position + 1 < properties.Length ? position + 1 : position;
                        break;
                    case ConsoleKey.Backspace:
                        exit = true;
                        break;
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void _editClass(ref int position, System.Reflection.PropertyInfo[] properties)
        {
            new ConsoleDataReader((IConsoleReadable)properties[position].GetValue(this.obj), Style).DataRead();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void _editEnum(ref int position, System.Reflection.PropertyInfo[] properties)
        {
            string[] enumOption = Enum.GetNames(properties[position].PropertyType);
            if (enumOption.Length == 0) return;

            int position2 = 0;
            bool back = false;
            ConsoleColorString buff = new ConsoleColorString();

            do
            {
                Console.Clear();

                for (int i = 0; i < enumOption.Length; i++)
                {
                    buff.Clear();
                    if (i == position2)
                    {
                        (Style.TextPrefixOfSelectItem + new ConsoleColorString(enumOption[i], Style.ValueColorOfSelectedItem, Style.ValueBackgroundColorOfSelectedItem)).ConsoleWriteLine();
                    }
                    else
                    {
                        (Style.TextPrefix + new ConsoleColorString(enumOption[i], Style.ValueColor, Style.ValueBackgroundColor)).ConsoleWriteLine();
                    }
                }
                Console.WriteLine();
                Style.ActionDescription.ConsoleWriteLine();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        properties[position].SetValue(obj, Enum.Parse(properties[position].PropertyType, enumOption[position2]));
                        back = true;
                        break;
                    case ConsoleKey.UpArrow:
                        position2 = position2 - 1 >= 0 ? position2 - 1 : 0;
                        break;
                    case ConsoleKey.DownArrow:
                        position2 = position2 + 1 < enumOption.Length ? position2 + 1 : position2;
                        break;
                    case ConsoleKey.Backspace:
                        back = true;
                        break;
                }
            } while (!back);
            back = false;
        }
    }
}
