using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConsoleToDoList.App
{
    public static partial class ReportsBuilder
    {
        public static ConsoleMenu.MenuStyle ViewMenuStyle = null;

        private static uint partsProgress = 0;
        private static uint levelOfProgress = 0;
        private static void printProgress()
        {
            Console.Clear();
            levelOfProgress++;
            if (partsProgress == levelOfProgress)
            {
                Console.WriteLine($"Progress : {levelOfProgress} / {partsProgress} Finish");
                Console.WriteLine("Press to continue");
                Console.ReadLine();
                levelOfProgress = 0;
            }
            else
            {
                Console.WriteLine($"Progress : {levelOfProgress} / {partsProgress}");
            }
        }

        private static bool saveAndShowRaport(string text)
        {
            try
            {
                string savePath;
                using (StreamWriter outputFile = new StreamWriter("Report.txt"))
                {
                    outputFile.WriteLine(text);
                    savePath = (outputFile.BaseStream as FileStream).Name;
                }


                Process firstProc = new Process();
                firstProc.StartInfo.FileName = "notepad.exe";
                firstProc.StartInfo.Arguments = savePath;
                firstProc.EnableRaisingEvents = true;

                firstProc.Start();
                firstProc.WaitForExit();
            }
            catch
            {
                Console.WriteLine("Fail");
                return false;
            }
            return true;
        }
    }
}
