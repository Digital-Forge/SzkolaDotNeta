using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    public interface ITag : IConsoleReadable
    {
        public string TagName { get; }
        public string TagDescription { get; }
    }
}
