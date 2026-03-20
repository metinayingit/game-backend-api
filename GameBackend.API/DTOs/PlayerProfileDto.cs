namespace GameBackend.API.DTOs
{
    public class PlayerProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Level { get; set; }
        public int XP { get; set; }
        public int Coins { get; set; }
        public List<InventoryItemDto> Inventory { get; set; } = new();
    }

    public class InventoryItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}