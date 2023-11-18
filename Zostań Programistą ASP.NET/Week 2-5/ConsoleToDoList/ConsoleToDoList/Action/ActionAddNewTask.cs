using ConsoleToDoList.App;
using ConsoleToDoList.ConsoleTerminal;


namespace ConsoleToDoList
{
    partial class Action
    {
        private void AddNewTask()
        {
            new TaskBuilder().Build(LogicCORE.Core.TasksData);
        }
    }
}
