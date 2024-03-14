namespace GameZone.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly string _gameImagesPath;

        public GameRepository(ApplicationDbContext context, IWebHostEnvironment webHost)
        {

            _context = context;
            _webHost = webHost;
            _gameImagesPath = $"{_webHost.WebRootPath}{FileSettings.GameImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(gd => gd.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            var game = _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(gd => gd.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
            return game;
        }

        public async Task Create(CreateGameViewModel model)
        {
            var CoverName = await SaveCover(model.Cover);

            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                Cover = CoverName,
                CategoryId = model.CategoryId,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameViewModel model)
        {
            var game = _context.Games
                .Include(g=>g.Devices)
                .SingleOrDefault(g=>g.Id==model.Id);

            if (game is null)
                return null;

            var hasNewCover = model.Cover;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover is not null)
            {
                game.Cover = await SaveCover(model.Cover!);
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover is not null)
                {
                    var Cover = Path.Combine(_gameImagesPath, oldCover);
                    File.Delete(Cover);
                }
                return game;
            }
            else
            {
                if (hasNewCover is not null)
                {
                    var Cover = Path.Combine(_gameImagesPath, game.Cover);
                    File.Delete(Cover);
                }
                return null;
            }

        }
        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null) return isDeleted;

            _context.Remove(game);

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_gameImagesPath,game.Cover);
                File.Delete(cover);
            }
            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile Cover)
        {
            var CoverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";
            var path = Path.Combine(_gameImagesPath, CoverName);
            using var Stream = File.Create(path);
            await Cover.CopyToAsync(Stream);
            return CoverName ;
        }

        
    }

}
