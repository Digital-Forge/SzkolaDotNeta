using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class LogicCORE<T> where T: class
    {
        [NonSerialized]
        public static LogicCORE<T> Core;

        public List<T> DataList;

        [NonSerialized]
        public static string DataPath = "RegisterData.dat";

        public LogicCORE()
        {
            Core = this;
            Load();
        }

        public LogicCORE(List<T> list)
        {
            if (list == null) throw new Exception("DataEnvironmentException");

            DataList = list;
            Core = this;
        }

        public void Save()
        {
            if (!Serialization<List<T>>.SerializationToFile(DataPath, DataList))
            {
                ConsoleAlert.Show("Save error\n Try to save manually", ConsoleColor.Red);
            }
        }

        public void Load()
        {
            var buff = Serialization<List< T >>.DeserializationFromFile(DataPath);

            if (buff == null)
            {
                ConsoleAlert.Show("Load error\n Try to load manually\n  Or continue if this first time", ConsoleColor.Yellow);
                DataList = new List<T>();
            }
            else DataList = buff;
        }
    }
}
