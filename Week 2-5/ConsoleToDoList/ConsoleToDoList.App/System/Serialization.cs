using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ConsoleToDoList.App
{
    public static class Serialization<T> where T: class
    {
        public static bool SerializationToFile(string path, T obj)
        {
            try
            {
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    bin.Serialize(fStream, obj);
                    fStream.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T DeserializationFromFile(string path)
        {
            T obj;

            try
            {
                BinaryFormatter binFormattter = new BinaryFormatter();
                using (Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    obj = (T)binFormattter.Deserialize(fStream);
                    fStream.Close();
                }
                return obj;
            }
            catch
            {
                return null;
            }
        }
    }
}
