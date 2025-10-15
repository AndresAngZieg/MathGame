using System.IO;
using System.Text.Json;

public static class ConfigManager
{
    private static string path = "config.json";

    public static GameConfig LoadConfig()
    {
        if (!File.Exists(path))
        {
            var defaultConfig = new GameConfig();
            SaveConfig(defaultConfig);
            return defaultConfig;
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GameConfig>(json);
    }

    public static void SaveConfig(GameConfig config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
