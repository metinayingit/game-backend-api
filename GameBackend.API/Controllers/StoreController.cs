using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GameBackend.API.Data;
using GameBackend.API.DTOs;
using GameBackend.API.Models;

namespace GameBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("items")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPost("add-coins")]
        public async Task<IActionResult> AddCoins(int amount)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);
            var player = await _context.Players.FindAsync(playerId);
            
            if (player == null) return NotFound();

            player.Coins += amount;
            await _context.SaveChangesAsync();

            return Ok(player.Coins);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyItem(BuyItemDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);

            var player = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player == null) return NotFound();

            var item = await _context.Items.FindAsync(request.ItemId);
            if (item == null) return NotFound();

            int totalCost = item.Price * request.Quantity;

            if (player.Coins < totalCost)
            {
                return BadRequest("Not enough coins.");
            }

            player.Coins -= totalCost;

            var existingInventoryItem = player.Inventory.FirstOrDefault(i => i.ItemId == item.Id);
            
            if (existingInventoryItem != null)
            {
                existingInventoryItem.Quantity += request.Quantity;
            }
            else
            {
                player.Inventory.Add(new PlayerItem
                {
                    ItemId = item.Id,
                    Quantity = request.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return Ok(player.Coins);
        }
    }
}