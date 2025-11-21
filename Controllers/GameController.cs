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
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (progression == null)
            {
                return NotFound(new ErrorResponse("No progressions found", "NO_PROGRESSION"));
            }

            return Ok(progression);
        }

        //POST /api/Game/Reset/{userId}
        [HttpPost("/Game/Reset/{userId}")]
        public async Task<ActionResult<Progression>> ResetProgression(int userId)
        {
            var progression = await _context.Progressions.FindAsync(userId);
            if (progression == null)
            {
                return NotFound(new ErrorResponse("no progression", "NO_PROGRESSION"));
            }if(progression.CalculateResetCost() > progression.count){

                return NotFound(new ErrorResponse("Not enough clicks to reset", "INSUFFICIENT_CLICKS"));

            }else{
                        if (progression.Count > progression.BestScore)
                            {
                                progression.BestScore = progression.Count;
                            }
                        progression.Count=0;
                        progression.Multiplier++;
                }

            }
        }
        

        // GET /api/Game/ResetCost/{userId}
        [HttpGet("Game/ResetCost/{userId}")]
        public async Task<ActionResult<int>> ResetCost(int userId)
        {
            var progression = await _context.Progressions
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (progression == null)
            {
                return NotFound(new ErrorResponse("No progressions found", "NO_PROGRESSION"));
            }

            int cost = progression.CalculateResetCost();
            return Ok(cost);
        }



        // GET /api/Game/Click/{userId}
        [HttpGet("Game/Click/{userId}")]
        public async Task<ActionResult<object>> Click(int userId)
        {
            var progression = await _context.Progressions
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (progression == null)
            {
                return BadRequest(new ErrorResponse("No progressions found", "NO_PROGRESSION"));
            }

            progression.Count += 1;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                count = progression.Count,
                multiplier = progression.Multiplier
            });
        }


    }
}


