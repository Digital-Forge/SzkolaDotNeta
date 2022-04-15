using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly Context _context;
        public ItemRepository(Context context)
        {
            _context = context;
        }

        public bool AddDepartmentToItem(Department department, Item item)
        {
            return AddDepartmentToItem(department.Id, item.Id);
        }

        public bool AddDepartmentToItem(string departmentId, string itemId)
        {
            return AddDepartmentToItem(new Guid(departmentId), new Guid(itemId));
        }

        public bool AddDepartmentToItem(Guid departmentId, Guid itemId)
        {
            try
            {
                _context.ItemsToDepartments.Add(new ItemToDepartment { DepartmentId = departmentId, ItemId = itemId });
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string AddSerialToItem(SerialItem serial, Item item)
        {
            return AddSerialToItem(serial, item.Id);
        }

        public string AddSerialToItem(SerialItem serial, string itemId)
        {
            return AddSerialToItem(serial, new Guid(itemId));
        }

        public string AddSerialToItem(SerialItem serial, Guid itemId)
        {
            try
            {
                serial.Id = new Guid();
                serial.IdItem = itemId;
                _context.SerialItems.Add(serial);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return serial.Id.ToString();
        }

        public string CreateItem(Item item)
        {
            try
            {
                item.Id = new Guid();
                _context.Items.Add(item);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return item.Id.ToString();
        }

        public bool DeleteItem(Item item)
        {
            try
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteSerial(SerialItem serial)
        {
            try
            {
                _context.SerialItems.Remove(serial);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public IQueryable<Department> GetDepartmentListByItem(string id)
        {
            return GetDepartmentListByItem(new Guid(id));
        }

        public IQueryable<Department> GetDepartmentListByItem(Guid id)
        {
            return _context.Items.Where(x => x.Id == id).SelectMany(x => x.Departments).Select(x => x.Department);
        }

        public Item GetItem(string id)
        {
            return GetItem(new Guid(id));
        }

        public Item GetItem(Guid id)
        {
            return _context.Items.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Item> GetItemList()
        {
            return _context.Items;
        }

        public IQueryable<ItemReservation> GetItemReservationListByItem(string id)
        {
            return GetItemReservationListByItem(new Guid(id));
        }

        public IQueryable<ItemReservation> GetItemReservationListByItem(Guid id)
        {
            return _context.ItemReservations.Where(x => x.ItemId == id);
        }

        public IQueryable<SerialItem> GetSerialItemsListByItem(string id)
        {
            return GetSerialItemsListByItem(new Guid(id));
        }

        public IQueryable<SerialItem> GetSerialItemsListByItem(Guid id)
        {
            return _context.Items.Where(x => x.Id == id).SelectMany(x => x.Serials);
        }

        public bool RemoveDepartmentFromItem(string departmentId, string itemId)
        {
            return RemoveDepartmentFromItem(new Guid(departmentId), new Guid(itemId));            
        }

        public bool RemoveDepartmentFromItem(Department department, Item item)
        {
            return RemoveDepartmentFromItem(department.Id, item.Id);
        }

        public bool RemoveDepartmentFromItem(Guid departmentId, Guid itemId)
        {
            var buff = _context.ItemsToDepartments.FirstOrDefault(x => x.DepartmentId == departmentId && x.ItemId == itemId);

            if (buff == null) return false;

            try
            {
                _context.ItemsToDepartments.Remove(buff);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdateItem(Item item)
        {
            try
            {
                _context.Items.Update(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
