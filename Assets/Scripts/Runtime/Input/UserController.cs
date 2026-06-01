using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayLength = 20f;
    [SerializeField] private Vector3 position;

    private CubeActions _cubeActions;
    private InputAction _pointerAction;
    private InputAction _clickAction;

    void Awake()
    {
        _cubeActions = new ();
        _pointerAction = _cubeActions.Pointer.Pointer;
        _clickAction = _cubeActions.Pointer.Click;
    }
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
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
        Debug.Log($"Pointer: {inputValue}");
    }

    private void OnEnable()
    {
        _cubeActions.Pointer.Enable();
        _clickAction.started += OnClickStarted;
        _clickAction.performed += OnClickPerformed;
        
        Debug.Log($"PointerMap enabled: {_cubeActions.Pointer.enabled} clickAction: {_clickAction.enabled} pointerAction: {_pointerAction.enabled}");
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("OnClickPerformed()");
        ReadPointerInput();
    }

    private void OnDisable()
    {
        _clickAction.started -= OnClickStarted;
        _cubeActions.Pointer.Disable();
    }
    
    private void OnClickStarted(InputAction.CallbackContext context)
    {
        Debug.Log("onClickStarted()");
    }
}
