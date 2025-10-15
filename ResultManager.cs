using ___MathGame2___;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

public static class ResultManager
{
    private static string filePath = "results.json";

    public static ObservableCollection<GameResult> LoadResults()
    {
        if (!File.Exists(filePath))
            return new ObservableCollection<GameResult>();

        string json = File.ReadAllText(filePath);
        var list = JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
        return new ObservableCollection<GameResult>(list);
    }

    public static void SaveResult(GameResult result)
    {
        var scores = LoadResults();
        scores.Add(result);

        // Guardar en archivo
        string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}
