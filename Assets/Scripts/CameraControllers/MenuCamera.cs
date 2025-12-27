using Managers;
using UnityEngine;

namespace CameraControllers {
    public class MenuCamera : MonoBehaviour
    {
        private void Start() {
            GameManager.Instance.GameStarted += OnGameStarted;
            GameManager.Instance.PlayerLeft += OnPlayerLeft;
        }

        private void OnGameStarted() {
            gameObject.SetActive(false);
        }

        private void OnPlayerLeft() {
            gameObject.SetActive(true);
        }
    }
}
