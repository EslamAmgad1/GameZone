namespace GameZone.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {

            _context = context;

        }

        public IEnumerable<SelectListItem> GetAllSelectList()
        {
            return _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .OrderBy(c => c.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
