using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleToDoList.App
{
    public static partial class ReportsBuilder
    {
        public static void TagReport()
        {
            bool reload;
            List<Tag> tagList = LogicCORE.Core.TagsData.TagsList.OrderBy(x => x.TagName).ToList();

            do
            {
                reload = false;
                ConsoleMenu menu = new ConsoleMenu(ViewMenuStyle);
                menu.AutoBackKeyButton = ConsoleKey.Backspace;

                menu.MenuTitle = new ConsoleColorString("Remove Tags From System", ConsoleColor.Yellow);


                menu.add(new ConsoleColorString("Serch"), () =>
                {
                    Console.Clear();
                    Console.Write("Tag Name : ");
                    string buff = Console.ReadLine();

                    if (!string.IsNullOrEmpty(buff))
                    {
                        Regex regex = new Regex($"{buff}");
                        tagList = tagList.Where(x => regex.IsMatch(x.TagName)).OrderBy(x => x.TagName).ToList();
                    }
                    reload = true;
                    menu.exitFunction();
                });
                menu.add(new ConsoleColorString("Back\n"), menu.exitFunction);

                foreach (var item in tagList)
                {
                    menu.add(new ConsoleColorString(item.TagName, ConsoleColor.Yellow), () =>
                    {
                        generateReportOfTag(item);
                        menu.exitFunction();
                    });
                }
                menu.show();
            } while (reload);
        }

        private static void generateReportOfTag(Tag tag)
        {
            levelOfProgress = 0;
            partsProgress = 9;
            printProgress();

            var listNormalizeTask = Node.NormalizationNodeToList(LogicCORE.Core.TasksData);
            int numberAllTask = listNormalizeTask.Count - 1;
            printProgress();
            listNormalizeTask = listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).TagsBag.TagsList.Exists(y => y.TagName == tag.TagName)).ToList();
            printProgress();
            int[] toDoTask = dataToDoTask(listNormalizeTask);
            printProgress();
            int[] finishTask = dataFinishTask(listNormalizeTask);
            printProgress();

            string report = @$"Total number of tasks with tag {"\""}{tag.TagName}{"\""} : {listNormalizeTask.Count} / {numberAllTask} -> {Math.Round(100.0 * listNormalizeTask.Count/ numberAllTask, 2)}% all

Total number of completed tasks with tag {"\""}{tag.TagName}{"\""} : {finishTask[0]} -> {Math.Round(100.0 * finishTask[0] / numberAllTask, 2)}%
    Priority : 
        NoPriority : {finishTask[1]} -> {Math.Round(100.0 * finishTask[1] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[1] / numberAllTask, 2)}% to all
        Hight : {finishTask[2]} -> {Math.Round(100.0 * finishTask[2] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[2] / numberAllTask, 2)}% to all
        Meddium : {finishTask[3]} -> {Math.Round(100.0 * finishTask[3] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[3] / numberAllTask, 2)}% to all
        Low : {finishTask[4]} -> {Math.Round(100.0 * finishTask[4] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[4] / numberAllTask, 2)}% to all

Total number of tasks with tag {"\""}{tag.TagName}{"\""} to be done : {toDoTask[0]} -> {Math.Round(100.0 * toDoTask[0] / numberAllTask, 2)}%
    Priority : 
        NoPriority : {toDoTask[1]} -> {Math.Round(100.0 * toDoTask[1] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[1] / numberAllTask, 2)}% to all
        Hight : {toDoTask[2]} -> {Math.Round(100.0 * toDoTask[2] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[2] / numberAllTask, 2)}% to all
        Meddium : {toDoTask[3]} -> {Math.Round(100.0 * toDoTask[3] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[3] / numberAllTask, 2)}% to all
        Low : {toDoTask[4]} -> {Math.Round(100.0 * toDoTask[4] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[4] / numberAllTask, 2)}% to all

";
            printProgress();

            var taskOverTime = listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false).
                                                 Where(x => (x.Data as TaskHook).Date != null && (x.Data as TaskHook).Date < DateTime.Now).Count();
            printProgress();

            if (taskOverTime != 0)
            {
                report += $"total number of delayed tasks with tag \"{tag.TagName}\" to be done : {taskOverTime}";
            }
            printProgress();

            if (saveAndShowRaport(report))
            {
                printProgress();
            }
        }
    }
}
