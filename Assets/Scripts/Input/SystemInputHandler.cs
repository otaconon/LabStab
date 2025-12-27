using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class SystemInputHandler : MonoBehaviour {
        public static SystemInputHandler Instance { get; private set; }

        public event Action PausePressed;

        private PlayerActions _actions;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _actions = new PlayerActions();
            _actions.Menus.Enable();
        }

        private void OnEnable() {
            _actions.Menus.Pause.performed += HandlePause;
        }

        private void OnDisable() {
            _actions.Menus.Pause.performed -= HandlePause;
        }

        private void HandlePause(InputAction.CallbackContext ctx) {
            PausePressed?.Invoke();
        }
    }
}