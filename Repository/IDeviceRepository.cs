namespace GameZone.Repository
{
    public interface IDeviceRepository
    {
        IEnumerable<SelectListItem> GetAllSelectList();
    }
}
