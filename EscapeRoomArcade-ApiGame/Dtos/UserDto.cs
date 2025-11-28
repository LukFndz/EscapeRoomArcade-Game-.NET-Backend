namespace EscapeRoomApi.Dtos
{
    public class UserDto
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Level { get; set; }
        public int TotalCoins { get; set; }
        public int TotalObjectsPushedOut { get; set; }
    }
}
