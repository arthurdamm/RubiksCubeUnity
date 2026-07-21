using System;
using UnityEngine;

public class CubeDirectionsResolver
{
    private readonly Transform _cubeTransform;
    
    // public CubeDirectionsResolver()

    public CubeDirection QuantizeAxialDirection(Vector3 axialDirection)
    {
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






}
