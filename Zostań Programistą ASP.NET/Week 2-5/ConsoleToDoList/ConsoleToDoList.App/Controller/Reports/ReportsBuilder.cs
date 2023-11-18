using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            string savePath;
            try
            {
                using (StreamWriter outputFile = new StreamWriter("Report.txt"))
                {
                    outputFile.WriteLine(text);
                    savePath = (outputFile.BaseStream as FileStream).Name;
                }
            }
            catch
            {
                Console.WriteLine("Fail");
                return false;
            }

            try
            {
                Process firstProc = new Process();
                firstProc.StartInfo.FileName = "notepad.exe";
                firstProc.StartInfo.Arguments = savePath;
                firstProc.EnableRaisingEvents = true;

                firstProc.Start();
                firstProc.WaitForExit();
            }
            catch
            {
                printProgress();
                Console.WriteLine($"Path to file : {savePath}");
                return false;
            }
            return true;
        }

        private static int[] dataFinishTask(List<Node> listNormalizeTask)
        {
            return new int[]
            {
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == true).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == true
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.NoPriority).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == true
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Hight).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == true
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Meddium).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == true
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Low).Count()
            };
        }

        private static int[] dataToDoTask(List<Node> listNormalizeTask)
        {
            return new int[]
            {
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.NoPriority).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Hight).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Meddium).Count(),
                listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false
                                  && (x.Data as TaskHook).Task.Priority == TaskPriority.Low).Count()
            };
        }
    }
}
