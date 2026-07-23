using UnityEngine;

public class CubeRayCaster
{
    private Camera _camera;
    private CubeBuilder _cubeBuilder;

    public CubeRayCaster(Camera camera, CubeBuilder cubeBuilder)
    {
        _camera = camera;
        _cubeBuilder = cubeBuilder;
    }

    public CubeLayerRotation? CastDrag(DragResult drag)
    {
        if (!Raycast(drag.Start, out RaycastHit startHit) || !Raycast(drag.Stop, out RaycastHit stopHit))
        {
            Debug.Log("CastDrag() Raycast failed.");
            return null;
        }
        if (!AreSameDirection(startHit.normal, stopHit.normal))
        {
            Debug.Log("CastDrag() normals not same!");
            return null;
        }
        DrawDragVectors(startHit, stopHit);
        Vector3 projectedDrag = stopHit.point - startHit.point;
        var startHitGameObject = startHit.transform.gameObject;
        var startHitCubieGridIndex = _cubeBuilder.CubieGridMapper.LocalPositionToGridIndex(AdjustHitPointForIndexPosition(startHit));
        var stopHitCubieGridIndex =
            _cubeBuilder.CubieGridMapper.LocalPositionToGridIndex(AdjustHitPointForIndexPosition(stopHit));
        Debug.Log(
            $"gameObject: {startHitGameObject.name} start: {startHitCubieGridIndex} stop: {stopHitCubieGridIndex}");

        var directionsResolver = new CubeDirectionsResolver(startHitGameObject.transform);
        var quantizedNormal = directionsResolver.QuantizeWorldToAxialDirection(startHit.normal);
        var dragMatchedAxis = directionsResolver.MatchProjectedDragAgainstPlaneAxes(projectedDrag, quantizedNormal);
        var selectedLayerAxis = directionsResolver.GetRemainingAxis(quantizedNormal.Axis, dragMatchedAxis.Axis);
        
        var crossRotation = Vector3.Cross(dragMatchedAxis.Vector, -quantizedNormal.Vector);
        var quantizedCrossRotation = directionsResolver.QuantizeAxialDirection(crossRotation);
        // if (directionsResolver.QuantizeAxialDirection(crossRotation) == 
        
        
        Logger.LogFields(new { quantizedNormal, dragMatchedAxis, selectedLayerAxis, crossRotation, quantizedCrossRotation});
        return new CubeLayerRotation(new CubeLayer(selectedLayerAxis, startHitCubieGridIndex[(int)selectedLayerAxis]), 90f * quantizedCrossRotation.Sign);
    }

    private static void DrawDragVectors(RaycastHit startHit, RaycastHit stopHit)
    {
        Debug.DrawRay(startHit.point, (stopHit.point - startHit.point).normalized * 5f, Color.indigo, 60f);
        Debug.DrawRay(startHit.point, startHit.normal * .5f, Color.red, 60f);
        Debug.DrawRay(stopHit.point, stopHit.normal * .5f, Color.red, 60f);
        
        Debug.DrawLine(startHit.point + startHit.normal * .01f, stopHit.point + stopHit.normal * .01f, Color.black,
            60f);
    }

    private Vector3 AdjustHitPointForIndexPosition(RaycastHit hit)
    {
        Vector3 adjustedPoint = hit.point;

        Vector3 adjustment = -hit.normal * (_cubeBuilder.CubieGridMapper.CubieBounds().size.x / 2f);
        adjustedPoint += adjustment;
        Debug.Log($"hitPoint: {hit.point} Adjusted: {adjustedPoint} N: {hit.normal} A: {adjustment} X: {_cubeBuilder.CubieGridMapper.CubieBounds().size.x / 2f} X: {_cubeBuilder.CubieGridMapper.CubieBounds().size.x}");
        adjustedPoint = hit.transform.gameObject.transform.InverseTransformPoint(adjustedPoint);
        Debug.Log($"Mapped to Local: {adjustedPoint}");
        return adjustedPoint;
    }

// private Vector3 FindClosestLocalDirectionMatching(Vector3 projectedDrag, RaycastHit hit)
//     {
//         Transform hitCubieTransform = hit.transform.gameObject.transform;
//         Debug.Log($"Hit transform directions: {hitCubieTransform.right}, {hitCubieTransform.up}, {hitCubieTransform.forward}");
//         var directions = new[] { hitCubieTransform.right, hitCubieTransform.up, hitCubieTransform.forward };
//         return FindMatchingDirection(projectedDrag, directions);
//     }

    private bool AreSameDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b) > .999f;
    }

    // internal Vector3 FindMatchingDirection(Vector3 direction, IReadOnlyList<Vector3> candidates)
    // {
    //     float maxDotProduct = Single.MinValue;
    //     Vector3 matchingDirection = Vector3.zero;
    //     foreach (var candidate in candidates)
    //     {
    //         float dotProduct = Vector3.Dot(candidate, direction);
    //         float absDotProduct = Mathf.Abs(dotProduct);
    //         if (absDotProduct > maxDotProduct)
    //         {
    //             maxDotProduct = absDotProduct;
    //             matchingDirection = candidate * Mathf.Sign(dotProduct);
    //         }
    //     }
    //     return matchingDirection;
    // }

    private bool Raycast(Vector2 screenPoint, out RaycastHit hit)
    {
        Ray ray = _camera.ScreenPointToRay(screenPoint);
        if (!Physics.Raycast(ray, out hit))
        {
            return false;
        }
        Debug.Log($"Raycast() {screenPoint} to {hit.point} / {hit.normal}");
        return true;
    }
    
    
}