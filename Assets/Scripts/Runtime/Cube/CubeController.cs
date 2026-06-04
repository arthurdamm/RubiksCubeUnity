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
    
    public void QueueRotateLayer(CubeLayer layer, float degrees)
    {
        _cubeModel.QueueRotateLayer(layer, degrees);
    }
    
    public void QueueRotateLayer(CubeLayerGeneral layer, float degrees)
    {
        _cubeModel.QueueRotateLayer(layer, degrees);
    }
}

