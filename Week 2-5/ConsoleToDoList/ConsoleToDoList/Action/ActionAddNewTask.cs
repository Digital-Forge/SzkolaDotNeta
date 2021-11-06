using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;


namespace ConsoleToDoList
{
    partial class Action
    {
        private void AddNewTask()
        {
            TaskHook buff = new TaskHook();

            ConsoleDataReader inputData = new ConsoleDataReader(buff.Task);
            inputData.DataRead();

            if (string.IsNullOrEmpty(buff.Task.Name))
            {
                return;
            }

            LogicCORE.Core.TasksData.CreateNewNode(buff);
        }
    }
}
