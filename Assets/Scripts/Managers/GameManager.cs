using System;
using UnityEngine;

namespace Managers {
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
    
        public event Action<bool> OnPauseStateChanged;

        private bool _isPaused = false;

        private void Awake() {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void TogglePause() {
            _isPaused = !_isPaused;

            Time.timeScale = _isPaused ? 0 : 1;

            if (_isPaused) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            OnPauseStateChanged?.Invoke(_isPaused);
        }
    }
}
