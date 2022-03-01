using System;
using TMPro;
using UnityEngine;

namespace Scenes.GameOver
{
    public class GameOverDataWriter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private void Awake()
        {
            text.text =
                $"Last survival time: {GameOverData.LastSurvivalDuration}s\nLongest survival time: {GameOverData.LongestSurvivalDuration}s";
        }
    }
}
