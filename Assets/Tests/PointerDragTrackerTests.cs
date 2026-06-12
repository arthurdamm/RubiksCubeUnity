using NUnit.Framework;
using UnityEngine;

public class PointerDragTrackerTests
{
    [Test]
    public void EndDrag_AboveThreshold_InvokesCallback()
    {
        CubeActions cubeActions = new();
        float dragTreshold = 10f;
        
        DragResult? result = null;
        var tracker = new PointerDragTracker(cubeActions.Pointer.Pointer,
            cubeActions.Pointer.Click, dragTreshold, (drag) => result = drag);
        
        tracker.BeginDrag(new Vector2(0, 0));
        tracker.EndDrag(new Vector2(10, 0));

        Assert.That(result.HasValue);
    }
}
