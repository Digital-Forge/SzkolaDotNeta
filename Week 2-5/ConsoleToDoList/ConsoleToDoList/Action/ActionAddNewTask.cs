using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;


namespace ConsoleToDoList
{
    partial class Action
    {
        private void AddNewTask()
        {
            TaskBuilder.Build(LogicCORE.Core.TasksData);
        }
    }
}
