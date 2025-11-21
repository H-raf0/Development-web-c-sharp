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
        [HttpGet("{UserId}")]
        public async Task<ActionResult<Progression>> GetUserProgression(int UserId)
        {
            var progression = await _context.Progressions
                .Where(p => p.UserId == UserId)
                .FirstOrDefaultAsync();

            if (progression == null)
            {
                return NotFound(new ErrorResponse("Progression not found", "PROGRESSION_NOT_FOUND"));
            }

            return Ok(progression);
        }
        
        private readonly ProgressionContext _context;
        public GameController(ProgressionContext ctx)
        {
            _context = ctx;
        }

        /*
        // GET /api/Game/Initialize/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<Progression>> InitializeProgression(int userId)
        {
            /*
            bool exists = await _context.Users.AnyAsync(u => u.Username == newUser.Username);
            if (exists)
            {
                return BadRequest(new ErrorResponse(
                    "Username already exists",
                    "USERNAME_EXISTS"
                ));
            }

            return ;
            * /
        }*/
    }
}
