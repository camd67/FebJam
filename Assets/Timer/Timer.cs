
using Scenes.GameOver;
using TMPro;
using UnityEngine;

namespace Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timerText;

        private float startTime;

        private void Awake()
        {
            startTime = Time.time;
        }

        void Update()
        {
            var elapsed = (int)(Time.time - startTime);
            GameOverData.UpdateSurvivalDuration(elapsed);
            timerText.text = $"{elapsed}s";
        }
    }
}
