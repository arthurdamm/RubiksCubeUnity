using UnityEngine;
using UnityEngine.InputSystem;

public class BasicDrag
{
    private float _dragThreshold;
    private float _dragThresholdSquared; // Should I just always compute this?
    
    private CubeActions _cubeActions;
    private CubeActions.PointerActions _pointerMap;
    
    private readonly InputAction _clickAction;
    private readonly InputAction _pointerAction;

    private bool _isDragging;
    private Vector2 _startPosition;

    public BasicDrag(CubeActions cubeActions, float dragThreshold)
    {
        // Debug.Log("BasicDrag::Constructor()");
        _dragThreshold = dragThreshold;
        _dragThresholdSquared = dragThreshold * dragThreshold;
        
        _cubeActions = cubeActions;
        _pointerMap = _cubeActions.Pointer;
        _clickAction = _pointerMap.Click;
        _pointerAction = _pointerMap.Pointer;
        EnableActions();
    }
    
    ~BasicDrag()
    {
        // Debug.Log("BasicDrag::~BasicDrag()");
        DisableActions();
    }

    private void EnableActions()
    {
        // Debug.Log("BasicDrag::EnableActions()");
        _clickAction.performed += OnClickPerformed;
    }
    
    private void DisableActions()
    {
        // Debug.Log("BasicDrag::DisableActions()");
        _clickAction.performed -= OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("BasicDrag::OnClickPerformed()");
        var pointerPosition = _pointerAction.ReadValue<Vector2>();

        if (!_isDragging)
        {
            _isDragging = true;
            _startPosition = pointerPosition;
        }
        else
        {
            _isDragging = false;
            if ((pointerPosition - _startPosition).sqrMagnitude < _dragThresholdSquared)
            {
                Debug.Log("BasicDrag::OnClickPerformed() dragThreshold failed");
                return;
            }
            ProcessDrag(pointerPosition);
        }
    }

    private void ProcessDrag(Vector2 endPosition)
    {
        var delta = endPosition - _startPosition;
        Debug.Log($"BasicDrag::ProcessDrag() start:{_startPosition} stop:{endPosition} delta:{delta.magnitude}");
    }
}