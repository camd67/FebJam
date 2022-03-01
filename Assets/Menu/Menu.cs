using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Transform playText;

        [SerializeField]
        private float amplitude;

        [SerializeField]
        private string sceneToLoad;

        private PlayerActionMaps playerActionMaps;

        private void Awake()
        {
            playerActionMaps ??= new PlayerActionMaps();
            playerActionMaps.Enable();
            playerActionMaps.Player.Fire.performed += context =>
            {
                SceneManager.LoadScene(sceneToLoad);
            };
        }

        private void OnDestroy()
        {
            playerActionMaps?.Disable();
        }

        void Update()
        {
            var scale = Mathf.Sin(Time.time) * amplitude + 1;
            playText.localScale = new Vector3(scale, scale, 1);
        }
    }
}
