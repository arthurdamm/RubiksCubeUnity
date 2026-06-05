using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Object = UnityEngine.Object;


public class CubeModel
{

    // Should these two be in this class?
    private readonly Transform _rootTransform;
    private readonly float _rotationDegreesPerSecond; 
    
    private readonly CubieGridMapper _cubieGridMapper;
    private Transform[,,] _cubies;

    private CubeLayer _layerToRotate;
    
    private float _degreesToRotateRemaining;
    private bool _isRotating;
    
    private readonly Queue<(CubeLayer, float)> _rotationQueue = new();
    
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
        Debug.Log($"QueueRotateLayer {layer} {degrees}");
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
        if (!_isRotating && _rotationQueue.Count > 0)
        {
            (_layerToRotate, _degreesToRotateRemaining) = _rotationQueue.Dequeue();
            _isRotating = true;
        }
    }
    
    private void RotateLayer(CubeLayer layer, float degrees)
    {
        RotateLayer(layer, degrees, LayerToWorldCenter(layer), LayerToLocalAxisRotation(layer));
    }

    private void RotateLayer(CubeLayer layer, float degrees, Vector3 worldCenter, Vector3 localAxis)
    {
        int xStart, xStop, yStart, yStop, zStart, zStop;
        (xStart, xStop, yStart, yStop, zStart, zStop) = _cubieGridMapper.GetStartStopForIteration(layer);
        
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
                    RotateCubie(_cubies[x, y, z], degrees, worldCenter, localAxis, dummyAxis.transform, dummyPoint.transform);
                }
            }
        }
        
        Object.Destroy(dummyAxis);
        Object.Destroy(dummyPoint);
    }

    private void RotateCubie(Transform cubie, float degrees, Vector3 worldCenter, Vector3 localAxis, Transform dummyAxis, Transform dummyPoint)
    {
        dummyAxis.transform.position = worldCenter;
        dummyAxis.transform.rotation = Quaternion.identity;
        dummyPoint.transform.position = cubie.position;
        
        dummyAxis.transform.Rotate(localAxis, degrees, Space.Self);
        cubie.position = dummyPoint.transform.position;
        cubie.Rotate(cubie.InverseTransformDirection(localAxis), degrees, Space.Self);
    }

    private Vector3 LayerToLocalAxisRotation(CubeLayer layer)
    {
        return layer.Axis switch
        {
            CubeAxis.X => _rootTransform.right,
            CubeAxis.Y => _rootTransform.up,
            CubeAxis.Z => _rootTransform.forward,
            _ => Vector3.zero
        };
    }

    private Vector3 LayerToWorldCenter(CubeLayer layer)
    {
        /*
         * For now average two opposing corners in the layer,
         * such as the first & last cubies of the row & col
         */
        int xStart, xStop, yStart, yStop, zStart, zStop;

        (xStart, xStop, yStart, yStop, zStart, zStop) = _cubieGridMapper.GetStartStopForIteration(layer);

        return (_cubies[xStart, yStart, zStart].position + _cubies[xStop, yStop, zStop].position) / 2;
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

