using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface {
    public class PauseMenu : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _leaveButton;

        private void Start() {
            _resumeButton.onClick.AddListener(Resume);
            _leaveButton.onClick.AddListener(Leave);
            
            GameManager.Instance.PauseStateChanged += Pause;
            gameObject.SetActive(false);
        }

        private void Resume() {
            gameObject.SetActive(false);
            GameManager.Instance.TogglePause();
        }

        private void Leave() {
            gameObject.SetActive(false);
            GameManager.Instance.LeaveGame();
        }

        private void Pause(bool isPaused) {
            gameObject.SetActive(isPaused);
        }
    }
}