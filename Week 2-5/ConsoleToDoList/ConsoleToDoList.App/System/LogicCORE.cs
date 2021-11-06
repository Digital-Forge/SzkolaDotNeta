using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class LogicCORE
    {
        [NonSerialized]
        public static LogicCORE Core;

        public Node TasksData;

        public string[] TagsData; // <<<--------------------------- to implementation modul

        [NonSerialized]
        public static string DataPath = "RegisterData.dat";

        public LogicCORE()
        {
            TasksData = new Node();
        }

        public LogicCORE(Node data)
        {
            if (data == null) throw new Exception("DataEnvironmentException");

            TasksData = data;
            Core = this;
        }

        public static void Save()
        {
            if (!Serialization<LogicCORE>.SerializationToFile(DataPath, Core))
            {
                ConsoleAlert.Show("Save error\n Try to save manually", ConsoleColor.Red);
            }
        }

        public static void Load()
        {
            var buff = Serialization<LogicCORE>.DeserializationFromFile(DataPath);

            if (buff == null)
            {
                ConsoleAlert.Show("Load error\n Try to load manually\n  Or continue if this first time", ConsoleColor.Yellow);
                Core = new LogicCORE();
            }
            else Core = buff;
        }
    }
}
