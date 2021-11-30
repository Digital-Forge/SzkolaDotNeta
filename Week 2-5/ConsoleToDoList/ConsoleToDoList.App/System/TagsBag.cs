using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class TagsBag
    {
        [Serializable]
        public class TagCell
        {
            private bool _lock;
            private Tag _tag;

            public Tag Tag { get => _tag; }
            public bool Lock { get => _lock; }

            public TagCell(Tag tag, bool toLock)
            {
                _tag = tag;
                _lock = toLock;
            }

            public void setLock(bool value)
            {
                _lock = value;
            }
        }

        private List<TagCell> _tagsList = new List<TagCell>();

        public List<Tag> TagsList { get => _tagsList.Select(x => x.Tag).ToList(); }

        public List<TagCell> cellsOfTagsList { get => _tagsList; }

        public void AddTag(Tag tag, bool toLock = false)
        {
            var findTag = _tagsList.Find(x => x.Tag.TagName.ToLower() == tag.TagName.ToLower());

            if (findTag == null)
            {
                _tagsList.Add(new TagCell(tag, toLock));
            }
            else
            {
                findTag.setLock(findTag.Lock ? true : toLock);
            }
        }

        public void AddTag(TagsBag bag, bool toLock = false)
        {
            foreach (var item in bag._tagsList)
            {
                AddTag(item.Tag, item.Lock || toLock);
            }
        }

        public bool RemoveTag(Tag tag, bool hardRemove = false)
        {
            var _tag = _tagsList.Find(x => x.Tag.TagName.ToLower() == tag.TagName.ToLower());
            if (_tag != null)
                if (!_tag.Lock || hardRemove)
                {
                    return _tagsList.Remove(_tag);
                }
            return false;
        }

        public bool RemoveTag(TagsBag bag, bool hardRemove = false)
        {
            bool removeStats = false;

            foreach (var tag in bag._tagsList)
            {
                var _tag = _tagsList.Find(x => x.Tag.TagName.ToLower() == tag.Tag.TagName.ToLower());
                if (_tag != null)
                    if (!_tag.Lock || hardRemove || tag.Lock)
                    {
                        _tagsList.Remove(_tag);
                        removeStats = true;
                    }
                return false;
            }
            return removeStats;
        }

        public void SortByLock()
        {
            _tagsList.OrderBy(x => x.Lock);
        }

        public void SortByTagName()
        {
            _tagsList.OrderBy(x => x.Tag.TagName);
        }
    }
}
