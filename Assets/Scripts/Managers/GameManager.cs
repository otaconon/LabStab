using System;
using System.Collections.Generic;
using Input;
using Networking;
using Player;
using Unity.Netcode;
using UnityEngine;
using UserInterface;

namespace Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        public event Action<bool> PauseStateChanged;
        public event Action GameStarted;
        public event Action PlayerLeft;

        public List<GameObject> Players { get; private set; }

        private bool _isPaused = false;


        private void Awake() {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            Instance.Players = new List<GameObject>();
        }

        public void AddPlayer(GameObject playerObj) {
            Players.Add(playerObj);
        }


        public async void HostGame() {
            string joinCode = await RelayManager.CreateRelay();
            if (joinCode == null) return;
                
            NetworkManager.Singleton.StartHost();
            StartGame();
        }

        public async void JoinGame(string gameCode) {
            if (string.IsNullOrEmpty(gameCode)) {
                Debug.LogWarning("Please enter a join code!");
                return;
            }

            bool success = await RelayManager.JoinRelay(gameCode);
            if (!success) return;
            NetworkManager.Singleton.StartClient();
            StartGame();
        }

        public void LeaveGame() {
            TogglePause();
            NetworkManager.Singleton.Shutdown();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SystemInputHandler.Instance.PausePressed -= TogglePause;
            
            PlayerLeft?.Invoke();
        }

        private void StartGame() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SystemInputHandler.Instance.PausePressed += TogglePause;
            
            GameStarted?.Invoke();
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

            PauseStateChanged?.Invoke(_isPaused);
        }
    }
}
