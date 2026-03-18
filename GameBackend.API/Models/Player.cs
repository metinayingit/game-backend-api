namespace GameBackend.API.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public int XP { get; set; } = 0;
        public int Coins { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<PlayerItem> Inventory { get; set; } = new List<PlayerItem>();
    }
}