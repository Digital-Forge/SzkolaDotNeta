using Domain.Utils;

namespace Domain.Models.System
{
    public class Dictionary : AuditableEntity
    {
        public DictionaryType Type { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public bool Active { get; set; }
        public bool Const { get; set; }
        public string Value { get; set; }
    }
}
