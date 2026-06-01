using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayLength = 20f;
    [SerializeField] private Vector3 position;

    [SerializeField] private float zoomScale = 2f;

    private CubeActions _cubeActions;
    private InputAction _pointerAction;
    private InputAction _clickAction;
    private InputAction _zoomAction;
    
    

    void Awake()
    {
        _cubeActions = new ();
        _pointerAction = _cubeActions.Pointer.Pointer;
        _clickAction = _cubeActions.Pointer.Click;
        _zoomAction = _cubeActions.Pointer.Zoom;
    }
    
    void Start()
    {
        position = new Vector3(Screen.width / 4, Screen.height / 3);
    }

    void Update()
    {
    }

    private void CastRay(Vector3 pointerPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.blueViolet);
        Debug.DrawLine(Vector3.zero, Vector3.right * rayLength, Color.brown, 60f);
        Vector3 start = ray.origin + Vector3.up * 3f;
        Debug.DrawLine(start, start + ray.direction * rayLength, Color.black);
    }
    
    private void ReadPointerInput()
    {
        var inputValue = _pointerAction.ReadValue<Vector2>();
        Debug.Log($"ReadValue Pointer: {inputValue}");
    }

    private void OnEnable()
    {
        _clickAction.performed += OnClickPerformed;
        _pointerAction.performed += OnPointerPerformed;
        _zoomAction.performed += OnZoomPerformed;
        _cubeActions.Pointer.Enable();
        Debug.Log($"PointerMap enabled: {_cubeActions.Pointer.enabled} clickAction: {_clickAction.enabled} pointerAction: {_pointerAction.enabled}");
    }
    
    private void OnDisable()
    {
        _cubeActions.Pointer.Disable();
        _clickAction.performed -= OnClickPerformed;
        _pointerAction.performed -= OnPointerPerformed;
        _zoomAction.performed -= OnZoomPerformed;
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnZoom {context}");
        var inputValue = context.ReadValue<Vector2>();
        float zoomAmount = inputValue.y * zoomScale;
        mainCamera.transform.Translate(Vector3.forward * zoomAmount, Space.Self);
        
    }

    private void OnPointerPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnPointer: {context}");
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("OnClickPerformed()");
        ReadPointerInput();
    }

    
    private void OnClickStarted(InputAction.CallbackContext context)
    {
        Debug.Log("onClickStarted()");
    }
}
