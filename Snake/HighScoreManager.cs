using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snake
{
    public static class HighScoreManager
    {
        private const string FileName = "highscores.json";

        public static async Task<List<GameScore>> LoadScoresAsync()
        {
            if (!File.Exists(FileName))
                return new List<GameScore>();

            using var stream = File.OpenRead(FileName);
            return await JsonSerializer.DeserializeAsync<List<GameScore>>(stream) ?? new List<GameScore>();
        }

        public static async Task SaveScoreAsync(int score, GameMode mode)
        {
            var scores = await LoadScoresAsync();

            scores.Add(new GameScore
            {
                Score = score,
                Mode = mode.ToString(),
                Date = DateTime.Now
            });

            var topScores = scores.OrderByDescending(x => x.Score).Take(5).ToList();
            await SaveToFileAsync(topScores);
        }

        public static async Task DeleteScoreAsync(GameScore scoreToDelete, List<GameScore> currentList)
        {
            if(currentList.Contains(scoreToDelete))
            {
                currentList.Remove(scoreToDelete);
                await SaveToFileAsync(currentList);
            }
        }

        private static async Task SaveToFileAsync(List<GameScore> scores)
        {
            using var stream = File.Create(FileName);
            await JsonSerializer.SerializeAsync(stream, scores);
        }
    }
}
