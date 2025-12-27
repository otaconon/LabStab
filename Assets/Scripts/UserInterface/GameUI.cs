using Networking;
using TMPro;
using UnityEngine;

namespace UserInterface {
    public class GameUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _joinCode;

        private void Start() {
            RelayManager.JoinedRelay += OnRelayJoined;
        }

        private void OnRelayJoined(string joinCode) {
            _joinCode.text = joinCode;
        } 
    }
}