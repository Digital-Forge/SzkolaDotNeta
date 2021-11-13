using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleToDoList.App
{
    class TagBag
    {
        private class TagCell
        {
            public Tag Tag;
            public bool Lock;

            public TagCell(Tag tag, bool toLock)
            {
                Tag = tag;
                Lock = toLock;
            }
        }

        private List<TagCell> _tagsList = new List<TagCell>();

        public List<Tag> TagsList { get => _tagsList.Select(x => x.Tag).ToList(); }

        public void AddTag(Tag tag, bool toLock = false)
        {
            var findTag = _tagsList.Find(x => x.Tag.TagName == tag.TagName);

            if (findTag == null)
            {
                _tagsList.Add(new TagCell(tag, toLock));
            }
            else
            {
                findTag.Lock = findTag.Lock ? true : toLock;
            }
        }

        public void AddTag(TagBag bag, bool toLock = false)
        {
            foreach (var item in bag._tagsList)
            {
                AddTag(item.Tag, item.Lock || toLock);
            }
        }

        public bool RemoveTag(Tag tag, bool hardRemove = false)
        {
            var _tag = _tagsList.Find(x => x.Tag.TagName == tag.TagName);
            if (_tag != null)
                if (!_tag.Lock || hardRemove)
                {
                    return _tagsList.Remove(_tag);
                }
            return false;
        }
    }
}
