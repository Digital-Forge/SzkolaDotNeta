using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;

namespace Application.Services.PickupPoint
{
    [AutoRegisterTransientService(typeof(IItemPickupPointService))]
    public class ItemPickupPointService : IItemPickupPointService
    {
        public Task<IComboBoxApi<Guid>.ResponsData> GetAvailableItemInServiceAsync(IComboBoxApi<Guid>.SearchOption option)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginationTable<IItemPickupPointService.ServiceTableModel>.TableData> GetItemsInServiceAsync(IItemPickupPointService.ServiceTableOption options)
        {
            throw new NotImplementedException();
        }
    }
}
