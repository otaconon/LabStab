using Input;
using Unity.Netcode;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : NetworkBehaviour {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private InputHandler inputHandler;

        [Header("Movement")]
        public float jumpForce = 5f;
        public float moveSpeed = 5f;

        [Header("Mouse")]
        public float sensitivity = 0.5f;
        public float smoothing = 2f;

        private CharacterController _controller;

        private Vector2 _mouseLook;
        private Vector2 _smoothY;
        private Vector3 _velocity;
    
        private void Awake() {
            _controller = GetComponent<CharacterController>();
            _controller.enableOverlapRecovery = true;
        }

        private void OnEnable() {
            if (IsSpawned && IsOwner)
                inputHandler.Enable();
        }

        private void OnDisable() {
            inputHandler.Disable();
        }
    
        public override void OnNetworkDespawn() {
            inputHandler.Disable();
        }

        private void Start () {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                inputHandler.Enable();
                
                if (cameraTransform == null) {
                    return;
                }
            
                var cam = cameraTransform.GetComponent<Camera>();
                if(cam) cam.enabled = true;
                var listener = cameraTransform.GetComponent<AudioListener>();

                if (listener) {
                    listener.enabled = true;
                }
            } else {
                if(cameraTransform != null) {
                    var cam = cameraTransform.GetComponent<Camera>();
                    if(cam) cam.enabled = false;
                    var listener = cameraTransform.GetComponent<AudioListener>();
                    if(listener) listener.enabled = false;
                }
                inputHandler.Disable();
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
            Vector2 lookInput = inputHandler.GetLookInput();
            var md = Vector2.Scale(lookInput, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            
            _smoothY.x = Mathf.Lerp(_smoothY.x, md.x, 1f / smoothing);
            _smoothY.y = Mathf.Lerp(_smoothY.y, md.y, 1f / smoothing);
            _mouseLook += _smoothY;
            _mouseLook.y = Mathf.Clamp(_mouseLook.y, -90f, 90f);

            cameraTransform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, transform.up);
        }

        private void HandleMovement() {
            Vector3 moveInput = inputHandler.GetMoveInput();
            Vector3 targetVelocity = transform.right * (moveInput.x * moveSpeed) + transform.forward * (moveInput.z * moveSpeed);
            
            _velocity = Vector3.Lerp(_velocity, targetVelocity, 5f * Time.deltaTime);
            
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void Jump() {
            if (!IsOwner) return;
            Debug.Log("Jump!");
        }
    }
}