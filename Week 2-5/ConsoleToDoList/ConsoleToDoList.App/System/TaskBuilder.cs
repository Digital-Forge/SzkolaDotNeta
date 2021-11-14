using ConsoleToDoList.ConsoleTerminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    public class TaskBuilder
    {
        private Node nodeHook;
        private TaskHook taskBuffer;
        private bool confirmStatus;
        private bool modifyStatus;

        public static IConsoleDataReader.DataConsoleReaderStyle DataConsoleReaderStyle = null;
        public static ConsoleMenu.MenuStyle MenuStyle = null;

        public TaskHook Build(Node node, bool modify = false)
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
                    return null;
                }
                confirmTaskMenu();
            } while (!confirmStatus);
            return taskBuffer;
        }


        private void confirmTaskMenu()
        {
            ConsoleMenu menuCreateTask = new ConsoleMenu(MenuStyle);

            menuCreateTask.MenuTitle = taskBuffer.Header();

            menuCreateTask.add(new ConsoleColorString("Back to edit"), menuCreateTask.exitFunction);
            
            if (modifyStatus)
            {
                menuCreateTask.add(new ConsoleColorString("Edit Data"), () => { taskBuffer.ModifyDate(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Edit Tag"), () => { taskBuffer.ModifyTag(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Add SubTask"), () => { taskBuffer.AddTask(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Save"), () => { cancel(); menuCreateTask.exitFunction(); });
            }
            else
            {
                menuCreateTask.add(new ConsoleColorString("Add Data"), () => { taskBuffer.ModifyDate(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Add Tag"), () => { taskBuffer.ModifyTag(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Add SubTask"), () => { taskBuffer.AddTask(); menuCreateTask.MenuTitle = taskBuffer.Header(); });
                menuCreateTask.add(new ConsoleColorString("Save"), () => { save(); menuCreateTask.exitFunction(); });
                menuCreateTask.add(new ConsoleColorString("Cancel"), () => { cancel(); menuCreateTask.exitFunction(); });
            }
            menuCreateTask.show();
        }

        private void save()
        {
            nodeHook.CreateNewNode(taskBuffer);
            confirmStatus = true;
        }

        private void cancel()
        {
            confirmStatus = true;
        }
    }
}
