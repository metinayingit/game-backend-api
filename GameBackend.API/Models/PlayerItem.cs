namespace GameBackend.API.Models
{
    public class PlayerItem
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}