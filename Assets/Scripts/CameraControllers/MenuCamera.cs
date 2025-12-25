using Managers;
using UnityEngine;

namespace CameraControllers {
    public class MenuCamera : MonoBehaviour
    {
        private void Start() {
            GameManager.Instance.OnGameStarted += OnGameStarted;
            GameManager.Instance.OnGameLeft += OnGameLeft;
        }

        private void OnGameStarted() {
            gameObject.SetActive(false);
        }

        private void OnGameLeft() {
            gameObject.SetActive(true);
        }
    }
}
