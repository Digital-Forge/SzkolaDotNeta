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
            _tagName = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Tag)) return false;
            return this.TagName == (obj as Tag).TagName; 
        }
    }
}
