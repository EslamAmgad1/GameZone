
namespace GameZone.ViewModels
{
    public class CreateGameViewModel : BaseGameViewModel
    {
        [AllowedExtensions(FileSettings.AllowedExtensions), MaxFileSize(FileSettings.MaxFileSizeInByte)]
        public IFormFile Cover { get; set; } = default!;
    }
}
