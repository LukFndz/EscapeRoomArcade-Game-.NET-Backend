using AutoMapper;
using EscapeRoomApi.Data;
using EscapeRoomApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscapeRoomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameDbContext _db;
        private readonly IMapper _mapper;

        public GameController(GameDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // POST api/game/start
        [HttpPost("start")]
        public async Task<IActionResult> StartRun([FromBody] CreateUserDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.PlayerName == dto.PlayerName);

            if (user == null)
                return NotFound("User does not exist.");

            var multiplier = 1f + (user.Level - 1) * 0.1f;

            return Ok(new
            {
                message = "Run started",
                currentLevel = user.Level,
                multiplier
            });
        }

        // POST api/game/end
        [HttpPost("end")]
        public async Task<IActionResult> EndRun([FromBody] EndRunRequestDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.PlayerName == dto.PlayerName);

            if (user == null)
                return NotFound("User not found.");

            user.TotalObjectsPushedOut += dto.ObjectsPushed;

            var multiplier = 1f + (user.Level - 1) * 0.1f;
            var finalEarned = (int)(dto.CoinsEarned * multiplier);

            user.TotalCoins += finalEarned;

            while (user.TotalCoins >= user.Level * 1000)
                user.Level++;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                finalCoins = user.TotalCoins,
                newLevel = user.Level,
                multiplierUsed = multiplier
            });
        }

        // GET api/game/leaderboard
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _db.Users
                .OrderByDescending(u => u.TotalCoins)
                .Take(20)
                .ToListAsync();

            return Ok(_mapper.Map<List<LeaderboardDto>>(users));
        }

        // GET api/game/user/{playerName}
        [HttpGet("user/{playerName}")]
        public async Task<IActionResult> GetUser(string playerName)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.PlayerName == playerName);

            if (user == null)
                return NotFound("User does not exist.");

            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
