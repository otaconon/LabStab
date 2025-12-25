using Networking;
using TMPro;
using Unity.Netcode;
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
            _hostButton.onClick.AddListener(async () => {
                string joinCode = await RelayManager.CreateRelay();
                if (joinCode == null) return;
                
                NetworkManager.Singleton.StartHost();

                _codeDisplay.text = $"Code: {joinCode}";
                HideButtons();
            });

            _clientButton.onClick.AddListener(async () => {
                string codeToJoin = _joinCodeInput.text;

                if (string.IsNullOrEmpty(codeToJoin)) {
                    Debug.LogWarning("Please enter a join code!");
                    return;
                }

                bool success = await RelayManager.JoinRelay(codeToJoin);
                if (!success) return;
                NetworkManager.Singleton.StartClient();
                HideButtons();
            });
        }

        private void HideButtons() {
            //gameObject.SetActive(false);
            _hostButton.gameObject.SetActive(false);
            _clientButton.gameObject.SetActive(false);
            _joinCodeInput.gameObject.SetActive(false);
        }
    }
}