using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IItemRepository
    {
        IQueryable<Item> GetItemList();
        Item GetItem(string id);
        Item GetItem(Guid id);
        IQueryable<SerialItem> GetSerialItemsListByItem(string id);
        IQueryable<SerialItem> GetSerialItemsListByItem(Guid id);
        IQueryable<Department> GetDepartmentListByItem(string id);
        IQueryable<Department> GetDepartmentListByItem(Guid id);
        IQueryable<ItemReservation> GetItemReservationListByItem(string id);
        IQueryable<ItemReservation> GetItemReservationListByItem(Guid id);
        string CreateItem(Item item);
        string AddSerialToItem(SerialItem serial, Item item);
        string AddSerialToItem(SerialItem serial, string itemId);
        string AddSerialToItem(SerialItem serial, Guid itemId);
        bool DeleteSerial(SerialItem serial);
        bool DeleteItem(Item item);
        bool UpdateItem(Item item);
        bool AddDepartmentToItem(Department department, Item item);
        bool AddDepartmentToItem(string departmentId, string itemId);
        bool AddDepartmentToItem(Guid departmentId, Guid itemId);
        bool RemoveDepartmentFromItem(Department department, Item item);
        bool RemoveDepartmentFromItem(string departmentId, string itemId);
        bool RemoveDepartmentFromItem(Guid departmentId, Guid itemId);
    }
}
