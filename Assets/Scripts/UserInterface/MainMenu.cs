using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private TMP_InputField _joinCodeInput;
        [SerializeField] private TextMeshProUGUI _codeDisplay;

        private string _joinCode;

        private void Start() {
            GameManager.Instance.OnGameStarted += () => gameObject.SetActive(false);
            GameManager.Instance.OnGameLeft += () => gameObject.SetActive(true);

            _hostButton.onClick.AddListener(HostGame);
            _clientButton.onClick.AddListener(JoinGame);
        }

        private void HostGame() {
            GameManager.Instance.HostGame();
            gameObject.SetActive(false);
        }

        private void JoinGame() {
            GameManager.Instance.JoinGame(_joinCodeInput.text); 
            gameObject.SetActive(false);
        }
    }
}