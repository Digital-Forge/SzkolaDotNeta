namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IItemPickupPointService
    {
        Task<IPaginationTable<ServiceTableModel>.TableData> GetItemsInServiceAsync(ServiceTableOption options);
        Task<IComboBoxApi<Guid>.ResponsData> GetAvailableItemInServiceAsync(IComboBoxApi<Guid>.SearchOption option);
    }
}
