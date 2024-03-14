namespace GameZone.Models
{
    public class GameDevice
    {
        public int GameId { get; set; }

        public int DeviceId { get; set; }

        public Game Game { get; set; } = default!;

        public Device Device { get; set; } = default!;
    }
}
