namespace EscapeRoomApi.Dtos
{
    public class EndRunRequestDto
    {
        public string PlayerName { get; set; } = string.Empty;
        public int ObjectsPushed { get; set; }
        public int CoinsEarned { get; set; }
        public bool Win { get; set; }
    }
}
