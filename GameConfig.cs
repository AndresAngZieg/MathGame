public class GameConfig
{
    public int TotalStars { get; set; } = 0;
    public List<string> UnlockedBackgrounds { get; set; } = new();
    public string CurrentBackground { get; set; } = "DarkBlue";
}
