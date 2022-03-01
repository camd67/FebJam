using System;

namespace Scenes.GameOver
{
    public class GameOverData
    {
        public static int LastSurvivalDuration { get; private set; }
        public static int LongestSurvivalDuration { get; private set; }

        public static void UpdateSurvivalDuration(int duration)
        {
            LastSurvivalDuration = duration;
            LongestSurvivalDuration = Math.Max(duration, LongestSurvivalDuration);
        }
    }
}
