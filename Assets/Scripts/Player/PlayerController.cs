using Input;
using Unity.Netcode;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerController : NetworkBehaviour {
        [Header("References")]
        [SerializeField] private Transform _cameraTransform;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpStrength = 5f;
        [SerializeField] private float _gravity = 5f;

        [Header("Mouse")]
        [SerializeField] private float _sensitivity = 0.5f;
        [SerializeField] private float _smoothing = 2f;

        private PlayerInputHandler _playerInputHandler;
        private CharacterController _controller;

        private Vector2 _mouseLook;
        private Vector2 _smoothY;
        private Vector3 _velocity;

        private void Awake() {
            _controller = GetComponent<CharacterController>();
            _controller.enableOverlapRecovery = true;

            _playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void OnEnable() {
            if (IsSpawned && IsOwner)
                _playerInputHandler.Enable();
        }

        private void OnDisable() {
            _playerInputHandler.Disable();
        }

        public override void OnNetworkDespawn() {
            _playerInputHandler.Disable();
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                _playerInputHandler.Enable();

                if (_cameraTransform == null) {
                    return;
                }

                var cam = _cameraTransform.GetComponent<Camera>();
                if (cam) cam.enabled = true;
                var listener = _cameraTransform.GetComponent<AudioListener>();

                if (listener) {
                    listener.enabled = true;
                }
            }
            else {
                if (_cameraTransform != null) {
                    var cam = _cameraTransform.GetComponent<Camera>();
                    if (cam) cam.enabled = false;
                    var listener = _cameraTransform.GetComponent<AudioListener>();
                    if (listener) listener.enabled = false;
                }

                _playerInputHandler.Disable();
            }
        }

        private void Update() {
            if (!IsOwner) {
                return;
            }

            HandleMovement();
            HandleLook();
        }

        private void HandleLook() {
            Vector2 lookInput = _playerInputHandler.GetLookInput();
            var md = Vector2.Scale(lookInput, new Vector2(_sensitivity * _smoothing, _sensitivity * _smoothing));

            _smoothY.x = Mathf.Lerp(_smoothY.x, md.x, 1f / _smoothing);
            _smoothY.y = Mathf.Lerp(_smoothY.y, md.y, 1f / _smoothing);
            _mouseLook += _smoothY;
            _mouseLook.y = Mathf.Clamp(_mouseLook.y, -90f, 90f);

            _cameraTransform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, transform.up);
        }

        private void HandleMovement() {
            Vector3 moveInput = _playerInputHandler.GetMoveInput();
            Vector3 targetVelocity = transform.TransformVector(moveInput) * _moveSpeed;

            _velocity = Vector3.Lerp(_velocity, targetVelocity, 5f * Time.deltaTime);
            
            if (_controller.isGrounded && _playerInputHandler.GetJumpInput()) {
                _velocity.y = _jumpStrength; 
            } else {
                _velocity.y -= _gravity * Time.deltaTime;
            }

            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}