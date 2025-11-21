using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        
    }
}
