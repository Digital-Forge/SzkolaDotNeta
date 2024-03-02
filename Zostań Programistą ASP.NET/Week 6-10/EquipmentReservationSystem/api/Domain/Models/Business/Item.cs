﻿using Domain.Models.Business.MiddleTabs;

namespace Domain.Models.Business
{
    public class Item : AuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Guid> Images { get; set; }
        public ICollection<Guid> AddictionFiles { get; set; }

        //Relations
        public virtual ICollection<ItemInstance> Instances { get; set; }
        public virtual ICollection<ItemToDepartment> Departments { get; set; }
    }
}
