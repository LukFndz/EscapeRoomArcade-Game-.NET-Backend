namespace EscapeRoomApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;

        public int TotalCoins { get; set; }
        public int TotalObjectsPushedOut { get; set; }

        public int Level { get; set; } = 1;
    }
}
