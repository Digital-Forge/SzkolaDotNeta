using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class Tag : ITag
    {
        private string _tagName;

        public string TagName { get => _tagName; }

        public Tag(string name)
        {

        }
    }
}
