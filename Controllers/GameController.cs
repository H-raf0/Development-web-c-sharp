using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using GameServerApi.Models;

namespace GameServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        
        private readonly ProgressionContext _context;
        public GameController(ProgressionContext ctx)
        {
            _context = ctx;
        }

        
        // GET /api/Game/Initialize/{userId}
        [HttpGet("Initialize/{userId}")]
        public async Task<ActionResult<Progression>> InitializeProgression(int userId)
        {
            
            bool exists = await _context.Progressions.AnyAsync(p => p.UserId == userId);
            if (exists)
            {
                return BadRequest(new ErrorResponse(
                    "Progression already exists",
                    "PROGRESSION_EXISTS"
                ));
            }

            try
            {
                var progression = new Progression(userId);
                _context.Progressions.Add(progression);
                await _context.SaveChangesAsync();
                return Ok(progression);
            }
            catch
            {
                return BadRequest(new ErrorResponse(
                    "Failed to initialize",
                    "INITIALIZATION_FAILED"
                ));
            }
        
        }

        // GET /api/Game/Progression/{userId}
        [HttpGet("Game/Progression/{userId}")]
        public async Task<ActionResult<Progression>> GetProgression(int userId)
        {
            var progression = await _context.Progressions
                .Where(p => p.userId == userId)
                .FirstOrDefaultAsync();

            if (progression == null)
            {
                return NotFound(new ErrorResponse("Progression not found", "PROGRESSION_NOT_FOUND"));
            }

            return Ok(progression);
        }
        
    }
}
