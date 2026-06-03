using System;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class CubeModel
{

    private Transform _rootTransform; // maybe should in a controller class
    private float _rotationDegreesPerSecond; 
    
    private CubieGridMapper _cubieGridMapper;
    private Transform[,,] _cubies;

    private CubeLayer _layerToRotate;
    private float _degreesToRotateRemaining;
    private bool _isRotating;
    
    private Queue<(CubeLayer, float)> _rotationQueue = new();
    
    // CubeModel still needs a CubieGridMapper for working with the indices, but its configuration is injected here
    public CubeModel(Transform[,,] cubies, CubieGridMapper cubieGridMapper, Transform rootTransform, float rotationDegreesPerSecond)
    {
        _cubies = cubies;
        _cubieGridMapper = cubieGridMapper;
        
        _rootTransform = rootTransform;
        _rotationDegreesPerSecond = rotationDegreesPerSecond;
    }
    
    public void ManualUpdate()
    {
        if (_isRotating)
        {
            AnimateRotateLayer();
        }

    }

    public void ResetCube(Quaternion orientation)
    {
        if (_isRotating) return;
        
        for (int x = 0; x < _cubies.GetLength(0); x++)
        {
            for (int y = 0; y < _cubies.GetLength(1); y++)
            {
                for (int z = 0; z < _cubies.GetLength(2); z++)
                {
                    _cubies[x, y, z].rotation = orientation;
                }
            }
        }
    }
    
    public void QueueRotateLayer(CubeLayer layer, float degrees)
    {
        _rotationQueue.Enqueue((layer, degrees));
        TryDequeRotation();
    }

    private void AnimateRotateLayer()
    {
        if (Mathf.Abs(_degreesToRotateRemaining) < 1e-6)
        {
            _degreesToRotateRemaining = 0f;
            _isRotating = false;
            RemapGridIndicesFromTransformPosition(_layerToRotate);
            TryDequeRotation();
            return;
        }
    
        float degreesToRotateNow = Mathf.Min(Mathf.Abs(_degreesToRotateRemaining), Time.deltaTime * _rotationDegreesPerSecond);
        degreesToRotateNow *= Mathf.Sign(_degreesToRotateRemaining);
        RotateLayer(_layerToRotate, degreesToRotateNow);
        _degreesToRotateRemaining -= degreesToRotateNow;
    }

    private void TryDequeRotation()
    {
        // CubeLayerRotation rot = new CubeLayerRotation {
        //     Layer = new CubeLayerGeneral(CubeAxis.X, 1),
        //     Degrees = 90f
        // };
        
        if (!_isRotating && _rotationQueue.Count > 0)
        {
            (_layerToRotate, _degreesToRotateRemaining) = _rotationQueue.Dequeue();
            _isRotating = true;
        }
    }

    private void RotateLayer(CubeLayer layer, float degrees)
    {
        int xStart, xStop, yStart, yStop, zStart, zStop;

        (xStart, xStop, yStart, yStop, zStart, zStop) = _cubieGridMapper.GetStartStopForIteration(layer);

        Transform centerTransform = _cubies[(xStart + xStop) / 2, (yStart + yStop) / 2, (zStart + zStop) / 2];
        Vector3 center = centerTransform.position;
        Vector3 axis = Vector3.up;

        Func<Transform, Vector3> f = null;

        switch ((int)layer / 3) // TODO: fix
        {
            case 0 : axis = _rootTransform.right;
                f = (t) => t.right; break;
            case 1 : axis = _rootTransform.forward; f = (t) => t.forward; break;
            case 2 : axis = _rootTransform.up; f = (t) => t.up; break;
        }

        var dummyAxis = new GameObject("DummyAxis");
        var dummyPoint = new GameObject("DummyPoint");
        dummyAxis.transform.position = _rootTransform.position;
        dummyPoint.transform.position = _rootTransform.position;
        dummyAxis.transform.SetParent(_rootTransform);
        dummyPoint.transform.SetParent(dummyAxis.transform);
        
        for (int x = xStart; x <= xStop; x++)
        {
            for (int y = yStart; y <= yStop; y++)
            {
                for (int z = zStart; z <= zStop; z++)
                {
                    // _cubies[x, y, z].RotateAround(center, axis, degrees);
                    Transform t = _cubies[x, y, z];
                    dummyAxis.transform.position = center;
                    dummyAxis.transform.rotation = Quaternion.identity;
                    
                    dummyPoint.transform.position = t.position;
                    
                    dummyAxis.transform.Rotate(axis, degrees, Space.Self);
                    
                    
                    t.position = dummyPoint.transform.position;
                    t.Rotate(t.InverseTransformDirection(axis), degrees, Space.Self);
                }
            }
        }

    }
    
    private void RemapGridIndicesFromTransformPosition(CubeLayer layer)
    {
        int xStart, xStop, yStart, yStop, zStart, zStop;

        (xStart, xStop, yStart, yStop, zStart, zStop) = _cubieGridMapper.GetStartStopForIteration(layer);
        Transform[,,] cubiesCopy = (Transform[,,])_cubies.Clone();

        for (int x = xStart; x <= xStop; x++)
        {
            for (int y = yStart; y <= yStop; y++)
            {
                for (int z = zStart; z <= zStop; z++)
                {
                    Vector3Int point = _cubieGridMapper.LocalPositionToGridIndex(_cubies[x, y, z].localPosition);
                    // Debug.Log($"copying [{x},{y},{z}] at local: {_cubies[x, y, z].localPosition}, world: {_cubies[x, y, z].position}, TO: [{point.x}, {point.y}, {point.z}]");
                    cubiesCopy[point.x, point.y, point.z] = _cubies[x, y, z];
                }
            }
        }
        _cubies = cubiesCopy;
    }

  
}

