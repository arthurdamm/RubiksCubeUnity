using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * CubeRayCaster
 * Takes DragResult and ray casts its onto the Cube
 * 1. Project drag onto cubie face
 * 2. Compare project vector to cubie cardinal dirs on that face
 * 3. draw resultant direction 
 * 
 */
public class CubeRayCaster
{
    private Camera _camera;

    public CubeRayCaster(Camera camera)
    {
        _camera = camera;
    }

    public void CastDrag(DragResult drag)
    {
        RaycastHit startHit, stopHit;

        if (!Raycast(drag.Start, out startHit) || !Raycast(drag.Stop, out stopHit))
        {
            Debug.Log("CastDrag() Raycast failed.");
            return;
        }
        Debug.DrawRay(startHit.point, (stopHit.point - startHit.point).normalized * 5f, Color.indigo, 60f);
        Debug.DrawRay(startHit.point, startHit.normal * .5f, Color.red, 60f);
        Debug.DrawRay(stopHit.point, stopHit.normal * .5f, Color.red, 60f);
        
        if (!AreSameDirection(startHit.normal, stopHit.normal))
        {
            Debug.Log("CastDrag() normals not same!");
            return;
        }

        Vector3 projectedDrag = stopHit.point - startHit.point;
        Debug.DrawLine(startHit.point + startHit.normal * .01f, stopHit.point + stopHit.normal * .01f, Color.black, 60f);
        
        // now need to align projectedDrag with a cardinal direction of the plane in local space
        Vector3 matchingLocalDirection = FindClosestLocalDirectionMatching(projectedDrag, startHit);

    }

    private Vector3 FindClosestLocalDirectionMatching(Vector3 projectedDrag, RaycastHit hit)
    {
        Transform hitCubieTransform = hit.transform;
        Debug.Log($"Hit transform directions: {hitCubieTransform.right}, {hitCubieTransform.up}, {hitCubieTransform.forward}");
        FindMatchingDirection(projectedDrag, new []{ hitCubieTransform.right, hitCubieTransform.up, hitCubieTransform.forward });
        var directions = new[] { hitCubieTransform.right, hitCubieTransform.up, hitCubieTransform.forward };
        FindMatchingDirection(projectedDrag, directions);

        return Vector3.zero;
    }

    private bool AreSameDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b) > .999f;
    }

    internal Vector3 FindMatchingDirection(Vector3 direction, IReadOnlyList<Vector3> candidates)
    {
        float maxDotProduct = Single.MinValue;
        Vector3 matchingDirection = Vector3.zero;
        foreach (var candidate in candidates)
        {
            float dotProduct = Vector3.Dot(candidate, direction);
            float absDotProduct = Mathf.Abs(dotProduct);
            if (absDotProduct > maxDotProduct)
            {
                maxDotProduct = absDotProduct;
                matchingDirection = candidate * Mathf.Sign(dotProduct);
            }
        }
        return matchingDirection;
    }

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