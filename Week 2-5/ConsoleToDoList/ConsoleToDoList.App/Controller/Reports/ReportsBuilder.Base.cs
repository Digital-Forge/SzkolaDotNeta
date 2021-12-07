using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleToDoList.App
{
    public static partial class ReportsBuilder
    {
        public static void BaseReport()
        {
            partsProgress = 8;
            levelOfProgress = 0;
            printProgress();

            var listNormalizeTask = Node.NormalizationNodeToList(LogicCORE.Core.TasksData);
            int numberAllTask = listNormalizeTask.Count - 1;
            printProgress();
            int[] toDoTask = br_dataToDoTask(listNormalizeTask);
            printProgress();
            int[] finishTask = br_dataFinishTask(listNormalizeTask);
            printProgress();

            string report = @$"Total number of tasks : {numberAllTask}

Total number of completed tasks : {finishTask[0]} -> {Math.Round(100.0 * finishTask[0] / numberAllTask, 2)}%
    Priority : 
        NoPriority : {finishTask[1]} -> {Math.Round(100.0 * finishTask[1] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[1] / numberAllTask, 2)}% to all
        Hight : {finishTask[2]} -> {Math.Round(100.0 * finishTask[2] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[2] / numberAllTask, 2)}% to all
        Meddium : {finishTask[3]} -> {Math.Round(100.0 * finishTask[3] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[3] / numberAllTask, 2)}% to all
        Low : {finishTask[4]} -> {Math.Round(100.0 * finishTask[4] / finishTask[0], 2)}% -> {Math.Round(100.0 * finishTask[4] / numberAllTask, 2)}% to all

Total number of tasks to be done : {toDoTask[0]} -> {Math.Round(100.0 * toDoTask[0] / numberAllTask, 2)}%
    Priority : 
        NoPriority : {toDoTask[1]} -> {Math.Round(100.0 * toDoTask[1] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[1] / numberAllTask, 2)}% to all
        Hight : {toDoTask[2]} -> {Math.Round(100.0 * toDoTask[2] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[2] / numberAllTask, 2)}% to all
        Meddium : {toDoTask[3]} -> {Math.Round(100.0 * toDoTask[3] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[3] / numberAllTask, 2)}% to all
        Low : {toDoTask[4]} -> {Math.Round(100.0 * toDoTask[4] / toDoTask[0], 2)}% -> {Math.Round(100.0 * toDoTask[4] / numberAllTask, 2)}% to all

Total number of completed tags : {LogicCORE.Core.TagsData.TagsList.Count()}

";
            printProgress();

            var taskOverTime = listNormalizeTask.Where(x => x.Data != null && (x.Data as TaskHook).FinishStatus == false).
                                                 Where(x => (x.Data as TaskHook).Date != null && (x.Data as TaskHook).Date < DateTime.Now).Count();
            printProgress();

            if (taskOverTime != 0)
            {
                report += $"total number of delayed tasks to be done : {taskOverTime}";
            }
            printProgress();

            if (saveAndShowRaport(report))
            {
                printProgress();
            }
        }

        private static int[] br_dataFinishTask(List<Node> listNormalizeTask)
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

        private static int[] br_dataToDoTask(List<Node> listNormalizeTask)
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
