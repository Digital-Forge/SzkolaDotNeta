using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.ConsoleTerminal
{
    public interface IConsoleDataReader
    {
        object GetReadObject { get; }
        Type GetReadObjectType { get; }
        DataConsoleReaderStyle Style { get; set; }

        void DataRead();

        class DataConsoleReaderStyle
        {
            public ConsoleColor? DescriptionColorOfSelectedItem = null;
            public ConsoleColor? DescriptionBackgroundColorOfSelectedItem = null;

            public ConsoleColor? ValueColorOfSelectedItem = null;
            public ConsoleColor? ValueBackgroundColorOfSelectedItem = null;

            public ConsoleColor? ValueColor = null;
            public ConsoleColor? ValueBackgroundColor = null;

            public ConsoleColorString TextPrefixOfSelectItem = new ConsoleColorString(" -> ");
            public ConsoleColorString TextMiddleFixOfSelectItem = new ConsoleColorString(" : ");

            public ConsoleColorString TextPrefix = new ConsoleColorString("    ");
            public ConsoleColorString TextMiddleFix = new ConsoleColorString(" : ");      
        }
    }
}
