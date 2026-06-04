using UnityEngine;

public class CubieGridMapper
{
    private readonly CubeLayout _cubeLayout;

    public CubieGridMapper(CubeLayout cubeLayout)
    {
        _cubeLayout = cubeLayout;
    }

    public Vector3 CubeBoundsSize()
    {
        return new Vector3(
            (_cubeLayout.CubieBounds.size.x + _cubeLayout.CubiePadding) * _cubeLayout.CubeSize,
            (_cubeLayout.CubieBounds.size.y + _cubeLayout.CubiePadding) * _cubeLayout.CubeSize,
            (_cubeLayout.CubieBounds.size.z + _cubeLayout.CubiePadding) * _cubeLayout.CubeSize);
    }
    
    public Vector3 GridIndexToLocalPosition(Vector3Int point)
    {
        return new Vector3(
            (point.x - _cubeLayout.CubeSize / 2) * (_cubeLayout.CubieBounds.size.x + _cubeLayout.CubiePadding),
            (point.y - _cubeLayout.CubeSize / 2) * (_cubeLayout.CubieBounds.size.y + _cubeLayout.CubiePadding),
            (point.z - _cubeLayout.CubeSize / 2) * (_cubeLayout.CubieBounds.size.z + _cubeLayout.CubiePadding)
            );
    }

    public Vector3Int LocalPositionToGridIndex(Vector3 point)
    {
        // Debug.Log($"LocalToIndex() cubeSize: {_cubeLayout.CubeSize}, {_cubeLayout.CubeSize / 2}");
        return new Vector3Int(
            Mathf.RoundToInt(point.x / (_cubeLayout.CubieBounds.size.x + _cubeLayout.CubiePadding) +
                _cubeLayout.CubeSize / 2),
            Mathf.RoundToInt(point.y / (_cubeLayout.CubieBounds.size.y + _cubeLayout.CubiePadding) +
                _cubeLayout.CubeSize / 2),
            Mathf.RoundToInt(point.z / (_cubeLayout.CubieBounds.size.z + _cubeLayout.CubiePadding) +
                _cubeLayout.CubeSize / 2));

    }
    
    public (int, int, int, int, int, int) GetStartStopForIteration(CubeLayer layer)
    {
        int xStart = 0, xStop = _cubeLayout.CubeSize - 1,
            yStart = 0, yStop = _cubeLayout.CubeSize - 1,
            zStart = 0, zStop = _cubeLayout.CubeSize - 1;
        
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

    public (int, int, int, int, int, int) GetStartStopForIteration(CubeLayerGeneral layer)
    {
        int xStart = 0,
            xStop = _cubeLayout.CubeSize - 1,
            yStart = 0,
            yStop = _cubeLayout.CubeSize - 1,
            zStart = 0,
            zStop = _cubeLayout.CubeSize - 1;
        
        switch (layer.Axis)
        {
            case CubeAxis.X: xStart = xStop = layer.Index; break;
            case CubeAxis.Y: yStart = yStop = layer.Index; break;
            case CubeAxis.Z: zStart = zStop = layer.Index; break;
        }
        
        return (xStart, xStop, yStart, yStop, zStart, zStop);
    }
    
    public (int, int, int, int, int, int) GetOppositeCornersInLayer(CubeLayerGeneral layer)
    {
        int xStart = 0,
            xStop = _cubeLayout.CubeSize - 1,
            yStart = 0,
            yStop = _cubeLayout.CubeSize - 1,
            zStart = 0,
            zStop = _cubeLayout.CubeSize - 1;
        
        switch (layer.Axis)
        {
            case CubeAxis.X: xStart = xStop = layer.Index; break;
            case CubeAxis.Y: yStart = yStop = layer.Index; break;
            case CubeAxis.Z: zStart = zStop = layer.Index; break;
        }
        
        return (xStart, xStop, yStart, yStop, zStart, zStop);
    }
    
    public CubeLayerGeneral CubeNotationToCubeLayer(CubeNotation layer)
    {
        switch (layer)
        {
            case CubeNotation.L:
                return new CubeLayerGeneral(CubeAxis.X, 0);
            case CubeNotation.X:
                return new CubeLayerGeneral(CubeAxis.X, _cubeLayout.CubeSize / 2);
            case CubeNotation.R:
                return new CubeLayerGeneral(CubeAxis.X, _cubeLayout.CubeSize - 1);
            case CubeNotation.F:
                return new CubeLayerGeneral(CubeAxis.Z, 0);
            case CubeNotation.Y:
                return new CubeLayerGeneral(CubeAxis.Z, _cubeLayout.CubeSize / 2);
            case CubeNotation.B:
                return new CubeLayerGeneral(CubeAxis.Z, _cubeLayout.CubeSize - 1);
            case CubeNotation.D:
                return new CubeLayerGeneral(CubeAxis.Y, 0);
            case CubeNotation.Z:
                return new CubeLayerGeneral(CubeAxis.Y, _cubeLayout.CubeSize / 2);
            case CubeNotation.U:
                return new CubeLayerGeneral(CubeAxis.Y, _cubeLayout.CubeSize - 1);
        }
        return new CubeLayerGeneral(CubeAxis.X, 0);
    }


}
