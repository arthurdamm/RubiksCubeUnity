using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(CubeBuilder))]
[RequireComponent(typeof(BoxCollider))]
public class CubeController : MonoBehaviour
{
    private CubeBuilder _cubeBuilder;
    private CubeModel _cubeModel;
    
    void Start()
    {
        _cubeBuilder = GetComponent<CubeBuilder>();
        _cubeModel = _cubeBuilder.BuildModel();
        
        // Collider needed for Pointer device raycasts, should this be elsewhere?
        var collider = GetComponent<BoxCollider>();
        collider.size = _cubeBuilder.CubieGridMapper.CubeBoundsSize();
    }
    
    // Update is called once per frame
    void Update()
    {
        _cubeModel.ManualUpdate();
    }

    public void OnReset()
    {
        _cubeModel.ResetCube(transform.rotation);
    }
    
    public void QueueRotateLayer(CubeNotation layer, float degrees)
    {
        _cubeModel.QueueRotateLayer(_cubeBuilder.CubieGridMapper.CubeNotationToCubeLayer(layer), degrees);
    }
}

