using UnityEngine;

public static class Rotations
{
    public static Vector3Int RotateX90Degrees(Vector3Int point, bool clockWise=true)
    {
        int directionMultiplier = clockWise ? 1 : -1;
        return new Vector3Int(point.x, directionMultiplier * point.z, directionMultiplier * -point.y);
    }
    
    public static Vector3Int RotateY90Degrees(Vector3Int point, bool clockWise=true)
    {
        int directionMultiplier = clockWise ? 1 : -1;
        return new Vector3Int(directionMultiplier * -point.z, point.y, directionMultiplier * point.x);
    }
    
    public static Vector3Int RotateZ90Degrees(Vector3Int point, bool clockWise=true)
    {
        int directionMultiplier = clockWise ? 1 : -1;
        return new Vector3Int(directionMultiplier * point.y, directionMultiplier * -point.x, point.z);
    }
}
