using AutoMapper;
using EscapeRoomApi.Data;
using EscapeRoomApi.Dtos;
using EscapeRoomApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscapeRoomArcade_ApiGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GameDbContext _context;
        private readonly IMapper _mapper;

        public UserController(GameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("login/{playerName}")]
        public async Task<IActionResult> Login(string playerName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PlayerName == playerName);

            if (user == null)
                return NotFound("User does not exist.");

            return Ok(user);
        }


        // POST api/user/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PlayerName))
                return BadRequest("Player name is required.");

            var existing = await _context.Users.FirstOrDefaultAsync(x => x.PlayerName == dto.PlayerName);
            if (existing != null)
                return BadRequest("Player already exists.");

            var user = new UserProfile
            {
                PlayerName = dto.PlayerName,
                Level = 1,
                TotalCoins = 0,
                TotalObjectsPushedOut = 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST api/user/updateProgress
        [HttpPost("updateProgress")]
        public async Task<IActionResult> UpdateProgress([FromBody] EndRunRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PlayerName == dto.PlayerName);
            if (user == null)
                return NotFound("User not found.");

            user.TotalCoins += dto.CoinsEarned;
            user.TotalObjectsPushedOut += dto.ObjectsPushed;

            user.Level = user.TotalCoins / 1000 + 1;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // GET api/user/leaderboard
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var top = await _context.Users
                .OrderByDescending(u => u.TotalCoins)
                .Take(10)
                .Select(u => new LeaderboardDto
                {
                    PlayerName = u.PlayerName,
                    TotalCoins = u.TotalCoins
                })
                .ToListAsync();

            return Ok(top);
        }

        [HttpGet("rank/{playerName}")]
        public async Task<IActionResult> GetRank(string playerName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PlayerName == playerName);
            if (user == null) return NotFound();

            int rank = await _context.Users
                .CountAsync(u => u.TotalCoins > user.TotalCoins) + 1;

            return Ok(new { rank = rank, coins = user.TotalCoins });
        }


    }
}
