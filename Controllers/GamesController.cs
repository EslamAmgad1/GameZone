namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IGameRepository _gameRepository;

        public GamesController(ICategoryRepository categoryRepository, IDeviceRepository deviceRepository, IGameRepository gameRepository)
        {

            _categoryRepository = categoryRepository;
            _deviceRepository = deviceRepository;
            _gameRepository = gameRepository;
        }
        public IActionResult Index()
        {
            var games = _gameRepository.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gameRepository.GetById(id);
            if(game is null)
                return NotFound();
            return View(game);
        }
        
        [HttpGet]
        public IActionResult Create()
        {

            CreateGameViewModel ViewModel = new()
            {
                Categories = _categoryRepository.GetAllSelectList(),
                Devices = _deviceRepository.GetAllSelectList()
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if(!ModelState.IsValid)
            {
                CreateGameViewModel ViewModel = new()
                {
                    Categories = _categoryRepository.GetAllSelectList(),
                    Devices = _deviceRepository.GetAllSelectList()
                };

                return View(ViewModel);
            }
            await _gameRepository.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gameRepository.GetById(id);
            if (game is null)
                return NotFound();
            EditGameViewModel EditViewModel = new()
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                Categories = _categoryRepository.GetAllSelectList(),
                Devices = _deviceRepository.GetAllSelectList(),
                SelectedDevices= game.Devices.Select(GD=>GD.DeviceId).ToList(),
                CurrentCover = game.Cover
            };
            return View(EditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                EditGameViewModel ViewModel = new()
                {
                    Categories = _categoryRepository.GetAllSelectList(),
                    Devices = _deviceRepository.GetAllSelectList()
                };

                return View(ViewModel);
            }
            var game =  await _gameRepository.Update(model);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameRepository.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
