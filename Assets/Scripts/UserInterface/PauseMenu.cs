using Input;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UserInterface {
    public class PauseMenu : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _leaveButton;

        private void Awake() {
            SystemInputHandler.Instance.OnPausePressed += OnPause;
            gameObject.SetActive(false);
        }

        private void Start() {
            _resumeButton.onClick.AddListener(() => {
                gameObject.SetActive(false);
                GameManager.Instance.TogglePause();
            }); 
            
            _leaveButton.onClick.AddListener(() => {
                NetworkManager.Singleton.Shutdown();
            });
        }

        private void OnPause() {
            gameObject.SetActive(true);
            GameManager.Instance.TogglePause();
        }
    }
}