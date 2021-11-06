using System;

namespace ConsoleToDoList.App
{
    [Serializable]
    class TaskHook : INodeDataIntegration
    {
        public Node Node { get; set; }

        public Task Task = new Task();
    }
}
