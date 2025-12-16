using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [Header("References")]
    public Transform cameraTransform;

    [Header("Movement")]
    public float jumpForce = 5f;
    public float moveSpeed = 5f;

    [Header("Mouse")]
    public float sensitivity = 0.5f;
    public float smoothing = 2f;

    private PlayerControls _input;
    private CharacterController _controller;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private Vector2 _mouseLook;
    private Vector2 _smoothY;
    private Vector3 _velocity;
    
    private void Awake() {
        _input = new PlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _input.Gameplay.Jump.performed += context => Jump();
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();
    
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();
        _controller.enableOverlapRecovery = true;
    }

    private void Update() {
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
        _lookInput = _input.Gameplay.Look.ReadValue<Vector2>();
        HandleMovement();
        HandleLook();
    }

    private void HandleLook() {
        var md = Vector2.Scale(_lookInput, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        _smoothY.x = Mathf.Lerp(_smoothY.x, md.x, 1f / smoothing);
        _smoothY.y = Mathf.Lerp(_smoothY.y, md.y, 1f / smoothing);
        _mouseLook += _smoothY;

        cameraTransform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, transform.up);
    }

    private void HandleMovement() {
        Vector3 targetVelocity = transform.right * (_moveInput.x * moveSpeed) + transform.forward * (_moveInput.y * moveSpeed);
        _velocity = Vector3.Lerp(_velocity, targetVelocity, 5f * Time.deltaTime);
        
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Jump() {
        Debug.Log("Jump!");
    }
}