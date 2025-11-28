namespace EscapeRoomApi.Dtos
{
    public class LeaderboardDto
    {
        public string PlayerName { get; set; } = string.Empty;
        public int TotalCoins { get; set; }
        public int Level { get; set; }
        public int TotalObjectsPushedOut { get; set; }
    }
}
