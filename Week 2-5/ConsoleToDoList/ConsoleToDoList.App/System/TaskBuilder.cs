using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    public static class TaskBuilder
    {
        private static Node nodeHook;
        private static TaskHook taskBuffer;
        private static bool confirmStatus;
        private static bool modifyStatus;

        public static IConsoleDataReader.DataConsoleReaderStyle DataConsoleReaderStyle = null;
        public static ConsoleMenu.MenuStyle MenuStyle = null;

        public static void Build(Node node, bool modify = false)
        {
            nodeHook = node;
            modifyStatus = modify;
            confirmStatus = false;
            taskBuffer = modifyStatus ? (TaskHook)nodeHook.Data: new TaskHook();

            ConsoleDataReader inputData = new ConsoleDataReader(taskBuffer.Task, DataConsoleReaderStyle);
            inputData.ActionDescription = new ConsoleColorString("Enter - edit/save, Arrow Up/Down, Backspace - To Continue");

            do
            {
                inputData.DataRead();
                if (string.IsNullOrEmpty(taskBuffer.Task.Name))
                {
                    confirmStatus = false;
                    return;
                }
                confirmTaskMenu();
            } while (!confirmStatus);
        }


        private static void confirmTaskMenu()
        {
            ConsoleMenu menuCreateTask = new ConsoleMenu(MenuStyle);

            menuCreateTask.MenuTitle = taskBuffer.Header();

            menuCreateTask.add(new ConsoleColorString("Back to edit"), menuCreateTask.exitFunction);
            
            if (modifyStatus)
            {
                menuCreateTask.add(new ConsoleColorString("Edit Data"), taskBuffer.ModifyDate);
                menuCreateTask.add(new ConsoleColorString("Edit Tag"), taskBuffer.ModifyTag);
                menuCreateTask.add(new ConsoleColorString("Add SubTask"), taskBuffer.AddTask);
                menuCreateTask.add(new ConsoleColorString("Save"), cancel);
            }
            else
            {
                menuCreateTask.add(new ConsoleColorString("Add Data"), taskBuffer.ModifyDate);
                menuCreateTask.add(new ConsoleColorString("Add Tag"), taskBuffer.ModifyTag);
                menuCreateTask.add(new ConsoleColorString("Add SubTask"), taskBuffer.AddTask);
                menuCreateTask.add(new ConsoleColorString("Save"), save);
                menuCreateTask.add(new ConsoleColorString("Cancel"), cancel);
            }
            menuCreateTask.show();
        }

        private static void save()
        {
            nodeHook.CreateNewNode(taskBuffer);
            confirmStatus = true;
        }

        private static void cancel()
        {
            confirmStatus = true;
        }
    }
}
