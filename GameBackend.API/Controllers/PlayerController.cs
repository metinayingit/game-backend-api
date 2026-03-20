using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GameBackend.API.Data;
using GameBackend.API.DTOs;

namespace GameBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult<PlayerProfileDto>> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int playerId = int.Parse(userIdClaim.Value);

            var player = await _context.Players
                .Include(p => p.Inventory)
                .ThenInclude(pi => pi.Item)
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player == null) return NotFound("Player not found.");

            var profileDto = new PlayerProfileDto
            {
                Id = player.Id,
                Username = player.Username,
                Level = player.Level,
                XP = player.XP,
                Coins = player.Coins,
                Inventory = player.Inventory.Select(pi => new InventoryItemDto
                {
                    ItemName = pi.Item.Name,
                    Quantity = pi.Quantity
                }).ToList()
            };

            return Ok(profileDto);
        }

        [HttpGet("leaderboard")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLeaderboard()
        {
            var topPlayers = await _context.Players
                .OrderByDescending(p => p.XP)
                .ThenByDescending(p => p.Level)
                .Take(10)
                .Select(p => new
                {
                    p.Username,
                    p.Level,
                    p.XP
                })
                .ToListAsync();

            return Ok(topPlayers);
        }
    }
}