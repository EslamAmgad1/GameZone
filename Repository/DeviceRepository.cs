namespace GameZone.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        public DeviceRepository(ApplicationDbContext context)
        {

            _context = context;

        }

        public IEnumerable<SelectListItem> GetAllSelectList()
        {
            return _context.Devices
                    .Select(d => new SelectListItem { Value = d.Id.ToString() , Text=d.Name} )
                    .OrderBy(s=>s.Text)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
