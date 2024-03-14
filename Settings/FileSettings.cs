using static System.Net.Mime.MediaTypeNames;

namespace GameZone.Settings
{
    public static class FileSettings
    {
        public const string GameImagesPath = "/Assets/Images/Games";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInByte = MaxFileSizeInMB * 1024 * 1024;

    }
}
