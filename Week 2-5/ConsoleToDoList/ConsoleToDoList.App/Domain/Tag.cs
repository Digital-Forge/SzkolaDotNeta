using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleToDoList.App
{
    [Serializable]
    class Tag : ITag
    {
        private string _tagName;
        private string _tagDescription;
        private DateTime? _tagDate;

        public string TagName { get => _tagName; }
        public string TagDescription { get => _tagDescription; }
        public DateTime? TagDate { get => _tagDate; }
    }
}
