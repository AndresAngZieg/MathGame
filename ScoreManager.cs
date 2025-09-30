using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace __WPFMathGame__
{
    public class ScoreManager
    {
        private const string ScoreFilePath = "score.txt";
        private static List<double> scores = LoadScores();

        private static List<double> LoadScores()
        {
            var result = new List<double>();

            if (File.Exists(ScoreFilePath))
            {
                foreach (var line in File.ReadAllLines(ScoreFilePath))
                {
                    if (double.TryParse(line, out double value))
                    {
                        result.Add(value);
                    }
                    // si la línea está vacía o no es un número, se ignora
                }
            }
            return result;
        }

        public void SaveScore(double score)
        {
            scores.Add(score);
            File.WriteAllLines(ScoreFilePath, scores.OrderByDescending(s => s).Select(s => s.ToString()));
        }

        public List<double> GetTopScores() => scores.Take(5).ToList();
    }
}
