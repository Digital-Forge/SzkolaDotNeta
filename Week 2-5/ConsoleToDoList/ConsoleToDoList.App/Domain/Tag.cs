using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class Tag : ITag
    {
        private string _tagName;
        private string _tagDescription;

        public string TagName { get => _tagName; }
        public string TagDescription { get => _tagDescription; }

        public Tag(string name)
        {

        }
    }
}
