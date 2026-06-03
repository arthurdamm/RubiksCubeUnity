using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(CubeBuilder))]
public class CubeController : MonoBehaviour
{


    private CubeBuilder _cubeBuilder;
    private CubeModel _cubeModel;
    
    private CubieGridMapper _cubieGridMapper;
    private Transform[,,] _cubies;

    private CubeLayer _layerToRotate;
    private float _degreesToRotateRemaining;
    private bool _isRotating;
    
    private Queue<(CubeLayer, float)> _rotationQueue = new();

    void Start()
    {
        

        _cubeBuilder = GetComponent<CubeBuilder>();
        _cubeModel = _cubeBuilder.BuildModel();
        
        var collider = GetComponent<BoxCollider>();
        collider.size = _cubieGridMapper.CubeSize();

        TryDequeRotation();
    }
    
    // Update is called once per frame
    void Update()
    {
        _cubeModel.ManualUpdate();
    }

    public void OnReset()
    {
    }
    
    public void QueueRotateLayer(CubeLayer layer, float degrees)
    {
        _rotationQueue.Enqueue((layer, degrees));
        TryDequeRotation();
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
}

