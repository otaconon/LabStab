using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMP_InputField joinCodeInput;
    public TextMeshProUGUI codeDisplay;

    private string _joinCode;

    private void Start()
    {
        hostButton.onClick.AddListener(async () => {
            
            string joinCode = await RelayManager.Instance.CreateRelay();

            if (joinCode != null)
            {
                NetworkManager.Singleton.StartHost();
                
                codeDisplay.text = $"Code: {joinCode}";
                HideButtons();
            }
        });

        clientButton.onClick.AddListener(async () => {
            string codeToJoin = joinCodeInput.text;

            if (string.IsNullOrEmpty(codeToJoin))
            {
                Debug.LogWarning("Please enter a join code!");
                return;
            }

            bool success = await RelayManager.Instance.JoinRelay(codeToJoin);
            if (success)
            {
                NetworkManager.Singleton.StartClient();
                HideButtons();
            }
        });
    }

    private void HideButtons()
    {
        hostButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        joinCodeInput.gameObject.SetActive(false);
    }
}
