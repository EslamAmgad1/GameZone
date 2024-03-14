namespace GameZone.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<SelectListItem> GetAllSelectList();
    }
}
