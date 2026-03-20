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
    public class GameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("kill-enemy")]
        public async Task<IActionResult> KillEnemy(KillEnemyDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);
            var player = await _context.Players
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player == null) return NotFound("Player not found.");

            player.XP += request.BaseXpReward;
            player.Coins += request.BaseCoinReward;

            int xpNeededForNextLevel = player.Level * 1000;
            bool leveledUp = false;

            if (player.XP >= xpNeededForNextLevel)
            {
                player.Level++;
                player.XP -= xpNeededForNextLevel;
                leveledUp = true;
            }

            string? droppedItemName = null;
            var random = new Random();

            if (random.Next(1, 101) <= 20)
            {
                var allItems = await _context.Items.ToListAsync();
                if (allItems.Any())
                {
                    var randomItem = allItems[random.Next(allItems.Count)];

                    var existingInventoryItem = player.Inventory.FirstOrDefault(i => i.ItemId == randomItem.Id);
                    if (existingInventoryItem != null)
                    {
                        existingInventoryItem.Quantity++;
                    }
                    else
                    {
                        player.Inventory.Add(new PlayerItem { ItemId = randomItem.Id, Quantity = 1 });
                    }
                    droppedItemName = randomItem.Name;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Congratulations! You killed {request.EnemyName}.",
                GainedXP = request.BaseXpReward,
                GainedCoins = request.BaseCoinReward,
                LeveledUp = leveledUp,
                CurrentLevel = player.Level,
                DroppedItem = droppedItemName ?? "Nothing dropped"
            });
        }

        [HttpPost("daily-reward")]
        public async Task<IActionResult> ClaimDailyReward()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);
            var player = await _context.Players.FindAsync(playerId);

            if (player == null) return NotFound("Player not found.");

            if (player.LastDailyRewardClaim.HasValue && (DateTime.UtcNow - player.LastDailyRewardClaim.Value).TotalHours < 24)
            {
                var hoursLeft = 24 - (DateTime.UtcNow - player.LastDailyRewardClaim.Value).TotalHours;
                return BadRequest($"You must wait {Math.Round(hoursLeft, 1)} hours to claim your next reward.");
            }

            player.Coins += 500;
            player.LastDailyRewardClaim = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Daily reward claimed! +500 Coins", CurrentCoins = player.Coins });
        }
    }
}