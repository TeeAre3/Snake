using System;

namespace Snake
{
    public class GameScore
    {
        public int Score { get; set; }
        public string Mode { get; set; }
        public DateTime Date { get; set; }

        public string DateDisplay => Date.ToString("HH:mm dd.MM");
    }
}
