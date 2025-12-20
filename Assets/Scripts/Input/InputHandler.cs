using UnityEngine;
using UnityEngine.Serialization;

namespace Input {
    public class InputHandler : MonoBehaviour
    {
        private PlayerActions _playerActions;

        public void Enable() {
            _playerActions.Enable(); 
        }

        public void Disable() {
            _playerActions.Disable(); 
        }

        private void Awake() {
            _playerActions = new PlayerActions();
        }
        
        private bool CanProcessInput() {
            return Cursor.lockState == CursorLockMode.Locked;
        }

        public Vector3 GetMoveInput() {
            if (!CanProcessInput())
                return Vector2.zero;

            var input = _playerActions.Gameplay.Move.ReadValue<Vector2>();
            var move = new Vector3(input.x, 0f, input.y);
             
            return Vector3.ClampMagnitude(move, 1f);
        }
        
        public Vector3 GetLookInput() {
            if (!CanProcessInput())
                return Vector2.zero;

            return _playerActions.Gameplay.Look.ReadValue<Vector2>();
        }
    }
}
