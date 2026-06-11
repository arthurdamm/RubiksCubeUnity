using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayLength = 5f;

    [SerializeField] private float zoomSpeed = 200f;
    [SerializeField] private float zoomScale = 1f;
    
    [SerializeField] private float lookRotationDegreesPerSecond = 90f;
    
    [Header("Drag")]
    [SerializeField] private float dragThreshold = 20f;

    private CubeController _cubeController;
    
    private CubeActions _cubeActions;
    private CubeActions.GameplayActions _gameplayMap;

    private InputAction _pointerAction;
    private InputAction _clickAction;
    private InputAction _zoomAction;
    
    private InputAction _upAction;
    private InputAction _downAction;
    private InputAction _leftAction;
    private InputAction _rightAction;
    private InputAction _forwardAction;
    private InputAction _backAction;
    private InputAction _counterClockwiseAction;
    private InputAction _lookAction;

    private float _cameraZoomDelta;

    private BasicDrag _drag;
    
    void Awake()
    {
        // Debug.Log("UserController::Awake()");
        _cubeController = GetComponent<CubeController>();
            
        
        _cubeActions = new ();
        _gameplayMap = _cubeActions.Gameplay;
        _counterClockwiseAction = _gameplayMap.CounterClockwise;
        _lookAction = _gameplayMap.Look;
        
        _drag = new BasicDrag(_cubeActions, dragThreshold);
        
        _pointerAction = _cubeActions.Pointer.Pointer;
        _clickAction = _cubeActions.Pointer.Click;
        _zoomAction = _cubeActions.Pointer.Zoom;
    }
    
    void Update()
    {
        ReadAndApplyLookInput();
        TryZoomCamera();
    }
    
    private void OnEnable()
    {
        // Debug.Log("UserController::OnEnable()");
        _gameplayMap.Up.performed += OnUpPerformed;
        _gameplayMap.Down.performed += OnDownPerformed;
        _gameplayMap.Left.performed += OnLeftPerformed;
        _gameplayMap.Right.performed += OnRightPerformed;
        _gameplayMap.Front.performed += OnFrontPerformed;
        _gameplayMap.Back.performed += OnBackPerformed;
        _gameplayMap.Reset.performed += OnResetPerformed;
        _gameplayMap.Enable();
        
        _clickAction.performed += OnClickPerformed;
        _pointerAction.performed += OnPointerPerformed;
        _zoomAction.performed += OnZoomPerformed;
        _cubeActions.Pointer.Enable();
    }
    
    private void OnDisable()
    {
        // Debug.Log("UserController::OnDisable()");
        _gameplayMap.Disable();
        _gameplayMap.Up.performed -= OnUpPerformed;
        _gameplayMap.Down.performed -= OnDownPerformed;
        _gameplayMap.Left.performed -= OnLeftPerformed;
        _gameplayMap.Right.performed -= OnRightPerformed;
        _gameplayMap.Front.performed -= OnFrontPerformed;
        _gameplayMap.Back.performed -= OnBackPerformed;
        _gameplayMap.Reset.performed -= OnResetPerformed;

        _cubeActions.Pointer.Disable();
        _clickAction.performed -= OnClickPerformed;
        _pointerAction.performed -= OnPointerPerformed;
        _zoomAction.performed -= OnZoomPerformed;
    }

    // Should this method have a better name? Should it change transform?
    private void ReadAndApplyLookInput()
    {
        if (_lookAction.IsPressed())
        {
            var inputValue = _lookAction.ReadValue<Vector2>();
            transform.Rotate(Vector3.up, inputValue.x * lookRotationDegreesPerSecond * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, inputValue.y * lookRotationDegreesPerSecond * Time.deltaTime, Space.World);
        }
    }
    
    private void TryZoomCamera()
    {
        var absDelta = Mathf.Abs(_cameraZoomDelta);
        if (absDelta < 1e-1)
        {
            return;
        }

        float zoomAmount = zoomSpeed * Time.deltaTime;
        zoomAmount = Mathf.Min(zoomAmount, absDelta) * Mathf.Sign(_cameraZoomDelta);
        mainCamera.transform.Translate(Vector3.forward * zoomAmount, Space.Self);
        // Debug.Log($"Translate {zoomAmount}");
        _cameraZoomDelta -= zoomAmount;

    }

    private void CastRay(Vector3 pointerPosition)
    {
        Debug.Log($"CastRay({pointerPosition})");
        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("NO HIT!");
            return;
        }
        Debug.Log($"HIT: {hit}");
        
        Debug.DrawLine(hit.point, hit.point + hit.normal * rayLength, Color.darkBlue, 5f);
        
    }
    
    private void QueueRotateLayer(CubeNotation layer, float degrees)
    {
        _cubeController.QueueRotateLayer(layer, degrees);
    }
    
    private void OnResetPerformed(InputAction.CallbackContext context)
    {
        _cubeController.OnReset();
    }
    
    private void OnUpPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnUpPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.U, 90 * signMultiplier);
    }
    
    private void OnDownPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnDownPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.D, 90 * signMultiplier);
    }

    private void OnLeftPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnLeftPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.L, 90 * signMultiplier);
    }

    private void OnRightPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnRightPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.R, 90 * signMultiplier);
    }

    private void OnFrontPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnFrontPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.F, 90 * signMultiplier);
    }

    private void OnBackPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnBackPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeNotation.B, 90 * signMultiplier);
    }
    
    private void OnDestroy()
    {
        // Debug.Log("UserController::OnDestroy()");
        _cubeActions.Dispose();
        _drag = null;
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnZoom {context}");
        var inputValue = context.ReadValue<Vector2>();
        float zoomAmount = inputValue.y * zoomScale;
        _cameraZoomDelta += zoomAmount;
    }

    private void OnPointerPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log($"OnPointer: {_clickAction.IsPressed()}");
        
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        // var pointerPosition = _pointerAction.ReadValue<Vector2>();
        // Debug.Log($"OnClickPerformed({_clickAction.IsPressed()}) at {pointerPosition} : {context}");
        // CastRay(pointerPosition);
    }
}
