using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GameBackend.API.Data;

namespace GameBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("consume/{itemId}")]
        public async Task<IActionResult> ConsumeItem(int itemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);
            var player = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player == null) return NotFound("Player not found.");

            var inventoryItem = player.Inventory.FirstOrDefault(i => i.ItemId == itemId);

            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                return BadRequest("You do not have this item in your inventory!");
            }

            inventoryItem.Quantity--;

            if (inventoryItem.Quantity == 0)
            {
                player.Inventory.Remove(inventoryItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Item successfully consumed!",
                RemainingQuantity = inventoryItem.Quantity
            });
        }
    }
}