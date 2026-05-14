using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubiePrefab;

    [SerializeField] private int _cubeSize = 3;
    [SerializeField] private Bounds _cubieBounds;
    [SerializeField] private float _cubiePadding = 0.1f;

    [SerializeField] private float rotationDegreesPerSecond = 30f;

    private CubieGridMapper _cubieGridMapper;
    
    private Transform[,,] _cubies;

    private CubeLayer _layerToRotate;
    private float _degreesToRotate = 0f;
    public bool _isRotating = false;
    private Queue<(CubeLayer, float)> _rotationQueue = new();
    
    void Start()
    {
        _cubieBounds = cubiePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        Logger.LogFields(new {_cubieBounds.center, _cubieBounds.size});

        _cubieGridMapper = new CubieGridMapper(_cubeSize, _cubiePadding, _cubieBounds);
        SpawnCubies();
        // RotateLayer(CubeLayer.L, 10);
        // RotateLayer(CubeLayer.R, 10);
        
        // QueueRotateLayer(CubeLayer.L, 90);
        // QueueRotateLayer(CubeLayer.X, 90);
        // QueueRotateLayer(CubeLayer.R, 90);
        QueueRotateLayer(CubeLayer.R, 90);
        QueueRotateLayer(CubeLayer.U, 90);
        QueueRotateLayer(CubeLayer.R, -90);
        QueueRotateLayer(CubeLayer.U, -90);
        //
        // QueueRotateLayer(CubeLayer.F, 90);
        // QueueRotateLayer(CubeLayer.Y, 90);
        // QueueRotateLayer(CubeLayer.B, 90);
        //
        // QueueRotateLayer(CubeLayer.D, 90);
        // QueueRotateLayer(CubeLayer.Z, 90);
        // QueueRotateLayer(CubeLayer.U, 90);
        
        // RotateLayer(CubeLayer.U, 90);
        // RotateLayer(CubeLayer.R, -90);
        // RotateLayer(CubeLayer.U, -90);
    }



    private void SpawnCubies()
    {
        _cubies = new Transform[_cubeSize, _cubeSize, _cubeSize];

        for (int x = 0; x < _cubeSize; x++)
        {
            for (int y = 0; y < _cubeSize; y++)
            {
                for (int z = 0; z < _cubeSize; z++)
                {
                    Vector3 spawnPosition = _cubieGridMapper.GridIndexToLocalPosition(new Vector3Int(x, y, z));
                    Debug.Log(spawnPosition);
                    spawnPosition += transform.position;
                    GameObject cubieGo = Instantiate(cubiePrefab, spawnPosition, Quaternion.identity, transform);
                    cubieGo.name = $"Cubie ({x}, {y}, {z})";
                    _cubies[x, y, z] = cubieGo.transform;
                    
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isRotating)
        {
            AnimateRotateLayer();
        }
        else if (_rotationQueue.Count > 0)
        {
            (_layerToRotate, _degreesToRotate) = _rotationQueue.Dequeue();
            _isRotating = true;
        }
    }

    private void QueueRotateLayer(CubeLayer layer, float degrees)
    {
        _rotationQueue.Enqueue((layer, degrees));
        // _layerToRotate = layer;
        // _degreesToRotate = degrees;
        // _isRotating = true;
    }

    private void AnimateRotateLayer()
    {
        if (_degreesToRotate < 1e-3)
        {
            _degreesToRotate = 0f;
            _isRotating = false;
            RemapGridIndices(_layerToRotate);
            return;
        }

        float degreesToRotateNow = Mathf.Min(_degreesToRotate, Time.deltaTime * rotationDegreesPerSecond);
        RotateLayer(_layerToRotate, degreesToRotateNow);
        _degreesToRotate -= degreesToRotateNow;
    }

    private void RotateLayer(CubeLayer layer, float degrees)
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

        Vector3 center = _cubies[(xStart + xStop) / 2, (yStart + yStop) / 2, (zStart + zStop) / 2].position;
        Vector3 axis = Vector3.up;

        switch ((int)layer / _cubeSize)
        {
            case 0 : axis = Vector3.right; break;
            case 1 : axis = Vector3.forward; break;
            case 2 : axis = Vector3.up; break;
        }
        

        for (int x = xStart; x <= xStop; x++)
        {
            for (int y = yStart; y <= yStop; y++)
            {
                for (int z = zStart; z <= zStop; z++)
                {
                    _cubies[x, y, z].RotateAround(center, axis, degrees);
                    
                }
            }
        }


    }

    private void RemapGridIndices(CubeLayer layer)
    {
        int xStart = 0, xStop = _cubeSize - 1, yStart = 0, yStop = _cubeSize - 1, zStart = 0, zStop = _cubeSize - 1;

        Transform[,,] cubiesCopy = (Transform[,,])_cubies.Clone();

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

        for (int x = xStart; x <= xStop; x++)
        {
            for (int y = yStart; y <= yStop; y++)
            {
                for (int z = zStart; z <= zStop; z++)
                {
                    Vector3Int point = _cubieGridMapper.LocalPositionToGridIndex(_cubies[x, y, z].localPosition);
                    Debug.Log($"copying [{x},{y},{z}] at local: {_cubies[x, y, z].localPosition}, world: {_cubies[x, y, z].position}, TO: [{point.x}, {point.y}, {point.z}]");
                    cubiesCopy[point.x, point.y, point.z] = _cubies[x, y, z];
                }
            }
        }
        _cubies = cubiesCopy;
    }

    // Rotates the indices of a layer by 90 degrees around primary axes
    private void RotateLayerIndices(CubeLayer layer, bool clockWise)
    {
        
        
    }

  
}

enum CubeLayer
{
    L,
    X,
    R,

    F,
    Y,
    B,
    
    D,
    Z,
    U,
}
