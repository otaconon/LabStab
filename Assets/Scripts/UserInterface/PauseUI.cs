using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface {
    public class PauseUI : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Canvas mainMenuCanvas;
        
        private PlayerActions _input;

        public void Start() {
            resumeButton.onClick.AddListener(() => {
                gameObject.SetActive(false);
            }); 
            
            leaveButton.onClick.AddListener(() => {
                NetworkManager.Singleton.Shutdown();
                mainMenuCanvas.gameObject.SetActive(true);
            });
        }
    }
}