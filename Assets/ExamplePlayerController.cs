using UnityEngine;

/// <summary>
/// Premade for XR Dev Programming.
/// Simple exposed methods for a first & third person character using a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class ExamplePlayerController : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float _maxCameraZoomDistance = 10;

    [SerializeField] private float _defaultCameraAngle = 30f;
    [SerializeField] private float _minCameraAngle = -37f;
    [SerializeField] private float _maxCameraAngle = 85f;
    [SerializeField] private float _firstPersonSnapDistance = 1.13f;
    [SerializeField] private float _cameraZoomSpeed = 50;
    [SerializeField] private float _cameraRotationSpeed = 150;
    [SerializeField] private float _cameraLerpSpeed = 8;

    [Header("Character Properties")]
    [SerializeField] private float _defaultMoveSpeed = 4;
    [SerializeField] private float _sprintMoveSpeedMultiplier = 2.0f;
    [SerializeField] private float _maxFallSpeed = 10f;

    private bool _useCamera = true;
    private float _currentZoomLevel;
    private float _currentCameraAngle;
    private float _cameraTargetHeight;

    private Vector2 _moveDirection;

    private float currentVerticalSpeed;
    private Transform _characterTransform;
    private Transform _camLookTransform;
    private Transform _camTransform;
    private CharacterController _characterController;
    private bool _isGrounded;

    private Vector3 CameraLocalDirection => Quaternion.AngleAxis(_currentCameraAngle, Vector3.right) * Vector3.back;

    /// <summary>
    /// Causes the player to move in a direction. X is Left/Right. Y is Forward/Back.
    /// </summary>
    public Vector2 MoveDirection { get => _moveDirection; set => _moveDirection = value.normalized; }

    /// <summary>
    /// Causes the player to sprint, moving in a direction by <see cref="_sprintMoveSpeedMultiplier"/>.
    /// </summary>
    public bool IsSprinting { get; set; }

    /// <summary>
    /// Causes the character to jump if they are grounded.
    /// </summary>
    public void Jump(float jumpPower = 5f)
    {
        if (_isGrounded)
            currentVerticalSpeed = jumpPower;
    }

    public void Turn(float xDirection) =>
        _characterTransform.Rotate(0, xDirection * _cameraRotationSpeed * Time.deltaTime, 0);

    public void VerticalLookAngle(float yDirection)
    {
        _currentCameraAngle -= yDirection * _cameraRotationSpeed * Time.deltaTime;
        _currentCameraAngle = Mathf.Clamp(_currentCameraAngle, _minCameraAngle, _maxCameraAngle);
    }

    public void Zoom(float zoomDirection)
    {
        _currentZoomLevel -= zoomDirection * _cameraZoomSpeed * Time.deltaTime;
        _currentZoomLevel = Mathf.Clamp(_currentZoomLevel, _firstPersonSnapDistance - 0.01f, _maxCameraZoomDistance);
    }

    private void Awake()
    {
        Camera mainCam = Camera.main;

        if (mainCam == null)
            mainCam = FindObjectOfType<Camera>();

        _useCamera = mainCam != null;

        if (_useCamera)
        {
            // Cache & Unparent Camera
            _camTransform = mainCam.transform;
            _camTransform.parent = null;
        }

        // Cache local Components.
        _characterTransform = transform;
        _characterController = GetComponent<CharacterController>();
        _cameraTargetHeight = _characterController.height * 0.8f; // 80% of the height

        // Create camera target
        if (_useCamera)
        {
            _camLookTransform = new GameObject("Camera Look Target").transform;
            _camLookTransform.parent = _characterTransform;
            _currentZoomLevel = _maxCameraZoomDistance * 0.5f;
            _currentCameraAngle = _defaultCameraAngle;
        }

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        // Set CameraTarget position in case cameraTargetHeight is changed at run time.
        _camLookTransform.localPosition = new Vector3(0, _cameraTargetHeight);

        Cursor.visible = false;

        CalculateGravity();
        MovementInput();
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        bool enableFirstPerson = _currentZoomLevel < _firstPersonSnapDistance;

        // Snap Camera zoom
        float currentZoom = enableFirstPerson ? 0.01f : _currentZoomLevel;

        // Camera Position.
        Vector3 localCamOffset = CameraLocalDirection * currentZoom;
        Vector3 targetCamPosition = _camLookTransform.TransformPoint(localCamOffset);
        _camTransform.position =
            enableFirstPerson ?
            targetCamPosition :
            Vector3.Lerp(_camTransform.position, targetCamPosition, _cameraLerpSpeed * Time.deltaTime);

        // Camera Rotation
        _camTransform.LookAt(_camLookTransform);
    }

    private void MovementInput()
    {
        float deltaTime = Time.deltaTime;

        // Run
        float movementSpeedMultiplier = (IsSprinting ? _sprintMoveSpeedMultiplier : 1) * deltaTime;

        // Set movement
        Vector3 velocity = new Vector3(MoveDirection.x * movementSpeedMultiplier,
            Mathf.Max(-_maxFallSpeed, currentVerticalSpeed) * deltaTime,
            MoveDirection.y * movementSpeedMultiplier);

        _isGrounded = _characterController.Move(_characterTransform.TransformDirection(velocity)) == CollisionFlags.CollidedBelow;
    }

    private void CalculateGravity() =>
        currentVerticalSpeed += Physics.gravity.y * Time.deltaTime;
}
