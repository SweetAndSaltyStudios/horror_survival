using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera = default;
    [SerializeField] private float _mouseSensitivity = default;

    private Quaternion _deltaRotation = default;
    private Vector3 _deltaPosition = default;
    private Vector2 _mouseLookDirection = default;
    private Vector2 _movementDirection = default;

    public const float MIN_LOOK_ANGLE = -90f;
    public const float MAX_LOOK_ANGLE = 90f;
    
    [SerializeField] private float _movementSpeed = default;
    [SerializeField] private float _jumpForce = default;
    
    private Rigidbody _rigidbody = default;
    private PlayerInputActions _inputActions = default;

    private void Awake() => Initialize();

    private void OnEnable() => _inputActions.Enable();
    private void Start() => Cursor.lockState = CursorLockMode.Locked;
    private void OnDisable() => _inputActions.Disable();

    private void Update()
    {
        _mainCamera.transform.Rotate(-Vector3.right * _mouseLookDirection.y * _mouseSensitivity * Time.deltaTime);    
    }

    private void FixedUpdate()
    {
        if(_mouseLookDirection != Vector2.zero)
        {
            _deltaRotation = Quaternion.Euler(Vector3.up * _mouseLookDirection.x * _mouseSensitivity * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(_rigidbody.rotation * _deltaRotation);
        }

        if(_movementDirection != Vector2.zero)
        {
            _deltaPosition = ((_rigidbody.transform.forward * _movementDirection.y) + (_rigidbody.transform.right * _movementDirection.x)) * _movementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + _deltaPosition);
        }
    }

    private void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _inputActions = new PlayerInputActions();

        _inputActions.Gameplay.Looking.started += Looking_started;
        _inputActions.Gameplay.Looking.performed += Looking_performed;
        _inputActions.Gameplay.Looking.canceled += Looking_canceled;

        _inputActions.Gameplay.Movement.started += Movement_started;
        _inputActions.Gameplay.Movement.performed += Movement_performed;
        _inputActions.Gameplay.Movement.canceled += Movement_canceled;
    }
    
    private void Movement_started(InputAction.CallbackContext context)
    {
    }
    private void Movement_performed(InputAction.CallbackContext context)
    {
        _movementDirection = _inputActions.Gameplay.Movement.ReadValue<Vector2>();   
    }
    private void Movement_canceled(InputAction.CallbackContext context)
    {
        _movementDirection = Vector2.zero;
    }
    private void Looking_started(InputAction.CallbackContext context)
    {
    }
    private void Looking_performed(InputAction.CallbackContext context)
    {
        _mouseLookDirection = _inputActions.Gameplay.Looking.ReadValue<Vector2>(); 
    }
    private void Looking_canceled(InputAction.CallbackContext context)
    {
        _mouseLookDirection = Vector2.zero;
    }
}
