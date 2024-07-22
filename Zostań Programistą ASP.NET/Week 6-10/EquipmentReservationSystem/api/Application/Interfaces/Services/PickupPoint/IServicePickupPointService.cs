using Domain.Utils;

namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IServicePickupPointService
    {
        Task<ServiceItemModel> GetServiceItemAsync(Guid id);
        Task UpdateItemAsync(ChangeStatusServiceItemModel model);
        Task<IPaginationTable<ServiceItemTableModel>.TableData> GetServiceItemsAsync(ServiceItemTableOption options);
        Task<IComboBoxApi<Guid>.ResponsData> GetAvailableServiceItemAsync(IComboBoxApi<Guid>.SearchOption search);
    }
}
