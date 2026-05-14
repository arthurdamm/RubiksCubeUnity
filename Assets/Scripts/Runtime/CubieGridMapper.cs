using UnityEngine;

public class CubieGridMapper
{
    private readonly float _cubiePadding = 0f;
    private readonly int _cubeSize = 1;
    private readonly Bounds _cubieBounds;

    public CubieGridMapper(int cubeSize, float cubiePadding, Bounds cubieBounds)
    {
        _cubeSize = cubeSize;
        _cubiePadding = cubiePadding;
        _cubieBounds = cubieBounds;
    }
    
    public Vector3 GridIndexToLocalPosition(Vector3Int point)
    {
        return new Vector3(
            (point.x - _cubeSize / 2) * (_cubieBounds.size.x + _cubiePadding),
            (point.z - _cubeSize / 2) * (_cubieBounds.size.y + _cubiePadding),
            (point.y - _cubeSize / 2) * (_cubieBounds.size.z + _cubiePadding));
    }

    public Vector3Int LocalPositionToGridIndex(Vector3 point)
    {
        Debug.Log($"LocalToIndex() cubeSize: {_cubeSize}, {_cubeSize / 2}");
        return new Vector3Int(
            Mathf.RoundToInt(point.x / (_cubieBounds.size.x + _cubiePadding) + _cubeSize / 2),
            Mathf.RoundToInt(point.z / (_cubieBounds.size.z + _cubiePadding) + _cubeSize / 2),
            Mathf.RoundToInt(point.y / (_cubieBounds.size.y + _cubiePadding) + _cubeSize / 2));

    }    
}
