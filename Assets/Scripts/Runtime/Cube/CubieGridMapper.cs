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

    public Vector3 CubeSize()
    {
        return new Vector3(
            (_cubieBounds.size.x + _cubiePadding) * _cubeSize,
            (_cubieBounds.size.y + _cubiePadding) * _cubeSize,
            (_cubieBounds.size.z + _cubiePadding) * _cubeSize);
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
    
    public (int, int, int, int, int, int) GetStartStopForIteration(CubeLayer layer)
    {
        int xStart = 0, xStop = _cubeSize - 1, yStart = 0, yStop = _cubeSize - 1, zStart = 0, zStop = _cubeSize - 1;
        
        switch (layer)
        {
            case CubeLayer.L: xStart = xStop = 0; break;
            case CubeLayer.X: xStart = xStop = 1; break;
            case CubeLayer.R: xStart = xStop = 2; break;
            
            case CubeLayer.F: yStart = yStop = 0; break;
            case CubeLayer.Y: yStart = yStop = 1; break;
            case CubeLayer.B: yStart = yStop = 2; break;
            
            case CubeLayer.D: zStart = zStop = 0; break;
            case CubeLayer.Z: zStart = zStop = 1; break;
            case CubeLayer.U: zStart = zStop = 2; break;
        }

        return (xStart, xStop, yStart, yStop, zStart, zStop);
    }


}
