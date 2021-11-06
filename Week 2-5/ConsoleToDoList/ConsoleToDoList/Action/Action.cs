using ConsoleToDoList.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList
{
    partial class Action
    {
        public Action()
        {
            init();
        }

        public void Run()
        {
            MainMenu();
        }

        private void init()
        {
            LogicCORE.Load();

            initStyle();
        }
    }
}
