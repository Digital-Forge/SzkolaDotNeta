using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    public static partial class ReportsBuilder
    {
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
    }
}
