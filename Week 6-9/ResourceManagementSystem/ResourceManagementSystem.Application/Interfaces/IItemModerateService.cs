using ResourceManagementSystem.Application.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.Interfaces
{
    public interface IItemModerateService
    {
        List<ItemOfListVM> GetItemsList();
        ItemVM GetCreateOfItemTemplate();
        ItemVM GetDetailsEditItem(string id);
    }
}
