using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: InternalsVisibleTo("Tests")]

public readonly struct DragResult
{
    public readonly Vector2 Start;
    public readonly Vector2 Stop;

    public Vector2 Delta => Stop - Start;

    public DragResult(Vector2 start, Vector2 stop)
    {
        Start = start;
        Stop = stop;
    }

    public override string ToString() => $"DragResult(Start={Start}, Stop={Stop}, Delta={Delta})";
}

public sealed class PointerDragTracker : IDisposable
{
    private readonly InputAction _pointerPositionAction;
    private readonly InputAction _pointerPressAction;
    private readonly float _dragThresholdSquared;
    private readonly Action<DragResult> _onDrag;
    
    private Vector2 _startPosition;

    public PointerDragTracker(InputAction pointerPositionAction, InputAction pointerPressAction, float dragThreshold, Action<DragResult> onDrag)
    {
        _pointerPositionAction = pointerPositionAction ?? throw new ArgumentNullException(nameof(pointerPositionAction));
        _pointerPressAction = pointerPressAction ?? throw new ArgumentNullException(nameof(pointerPressAction));
        _onDrag = onDrag ?? throw new ArgumentNullException(nameof(onDrag));
        _dragThresholdSquared = dragThreshold * dragThreshold;
        EnableActions();
    }
    
    public void Dispose()
    {
        DisableActions();
    }

    private void EnableActions()
    {
        _pointerPressAction.started += OnPressStarted;
        _pointerPressAction.canceled += OnPressCanceled;
    }
    
    private void DisableActions()
    {
        _pointerPressAction.started -= OnPressStarted;
        _pointerPressAction.canceled -= OnPressCanceled;
    }

    private void OnPressStarted(InputAction.CallbackContext context)
    {
        BeginDrag(_pointerPositionAction.ReadValue<Vector2>());
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        EndDrag(_pointerPositionAction.ReadValue<Vector2>());
    }
    
    internal void BeginDrag(Vector2 position)
    {
        _startPosition = position;
    }

    internal void EndDrag(Vector2 position)
    {
        if ((position - _startPosition).sqrMagnitude >= _dragThresholdSquared)
        {
            _onDrag(new DragResult(_startPosition, position));
        }
    }
}