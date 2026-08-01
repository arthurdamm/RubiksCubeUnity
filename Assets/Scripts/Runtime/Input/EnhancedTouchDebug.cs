using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class EnhancedTouchDebug : MonoBehaviour
{
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            int i = 0;
            string phaseName = touch.phase switch
            {
                TouchPhase.Began => "BEGAN",
                TouchPhase.Canceled => "CANCELED",
                TouchPhase.Ended => "ENDED",
                TouchPhase.Moved => "MOVED",
                TouchPhase.None => "NONE",
                TouchPhase.Stationary => "STATIONARY",
                _ => throw new ArgumentOutOfRangeException(nameof(touch), $"Unexpected value: {touch}"),
            };
            Debug.Log($"Touch[{i}] {Time.frameCount} {Time.time} {phaseName} {touch.screenPosition} ({touch.startScreenPosition}");
        }
        
        // foreach (var touch in Touch.activeFingers)
        // {
        //     int i = 0;
        //     string phaseName = touch.phase switch
        //     {
        //         TouchPhase.Began => "BEGAN",
        //         TouchPhase.Canceled => "CANCELED",
        //         TouchPhase.Ended => "ENDED",
        //         TouchPhase.Moved => "MOVED",
        //         TouchPhase.None => "NONE",
        //         TouchPhase.Stationary => "STATIONARY",
        //         _ => throw new ArgumentOutOfRangeException(nameof(touch), $"Unexpected value: {touch}"),
        //     };
        //     Debug.Log($"Touch[{i}] {Time.frameCount} {Time.time} {phaseName} {touch.screenPosition} ({touch.startScreenPosition}");
        // }
    }
}
