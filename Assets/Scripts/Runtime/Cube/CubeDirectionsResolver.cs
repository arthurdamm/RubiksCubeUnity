using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeDirectionsResolver
{
    private readonly Transform _cubeTransform;

    public CubeDirectionsResolver(Transform cubeTransform)
    {
        _cubeTransform = cubeTransform;
    }

    public CubeDirection QuantizeAxialDirection(Vector3 axialDirection)
    {
        Debug.Log($"QuantizeAxialDirection() {axialDirection}");
        if (axialDirection.sqrMagnitude < .9f)
        {
            throw new ArgumentException("Direction is too small.", nameof(axialDirection));
        }

        float absX = Mathf.Abs(axialDirection.x);
        float absY = Mathf.Abs(axialDirection.y);
        float absZ = Mathf.Abs(axialDirection.z);
        if (absX > absZ && absX > absY)
        {
            return new CubeDirection(CubeAxis.X, axialDirection.x > 0f ? 1 : -1);
        }

        if (absY > absZ)
        {
            return new CubeDirection(CubeAxis.Y, axialDirection.y > 0f ? 1 : -1);
        }
        return new CubeDirection(CubeAxis.Z, axialDirection.z > 0f ? 1 : -1);
    }

    public CubeDirection QuantizeWorldToAxialDirection(Vector3 worldDirection)
    {
        Debug.Log($"QuantizeWorldToAxialDirection() {worldDirection}");
        return QuantizeAxialDirection(_cubeTransform.InverseTransformDirection(worldDirection));
    }

    public CubeDirection MatchProjectedDragAgainstPlaneAxes(Vector3 projectedDrag, CubeDirection normal)
    {
        var candidates = normal.Axis switch
        {
            CubeAxis.X => new[] { Vector3.up, Vector3.forward },
            CubeAxis.Y => new[] { Vector3.right, Vector3.forward },
            CubeAxis.Z => new[] { Vector3.right, Vector3.up },
            _ => throw new ArgumentOutOfRangeException()
        };
        var matchingAxialDirection = FindMatchingDirection(projectedDrag, candidates);
        Debug.Log($"MatchProjectedDragAgainstPlaneAxes() matchingAxialDirection: {matchingAxialDirection}");
        return QuantizeAxialDirection(matchingAxialDirection);
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

    public CubeAxis GetRemainingAxis(CubeAxis first, CubeAxis second)
    {
        if (first == second)
        {
            throw new ArgumentException("The two axes must be different.");
        }

        if (first != CubeAxis.X && second != CubeAxis.X)
        {
            return CubeAxis.X;
        } else if (first != CubeAxis.Y && second != CubeAxis.Y)
        {
            return CubeAxis.Y;
        }

        return CubeAxis.Z;
    }
}
