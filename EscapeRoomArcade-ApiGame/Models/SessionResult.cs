namespace EscapeRoomApi.Models
{
    public class SessionResult
    {
        public string Token { get; set; } = string.Empty;
        public int CoinsEarned { get; set; }
        public int EnemiesKilled { get; set; }
    }
}
