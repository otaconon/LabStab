using Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface {
    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private TMP_InputField joinCodeInput;
        [SerializeField] private TextMeshProUGUI codeDisplay;

        private string _joinCode;

        private void Start() {
            hostButton.onClick.AddListener(async () => {
                string joinCode = await RelayManager.CreateRelay();
                if (joinCode == null) return;
                
                NetworkManager.Singleton.StartHost();

                codeDisplay.text = $"Code: {joinCode}";
                HideButtons();
            });

            clientButton.onClick.AddListener(async () => {
                string codeToJoin = joinCodeInput.text;

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
            hostButton.gameObject.SetActive(false);
            clientButton.gameObject.SetActive(false);
            joinCodeInput.gameObject.SetActive(false);
        }
    }
}