using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubiePrefab;

    [SerializeField] private int _cubeSize = 3;
    [SerializeField] private Bounds _cubieBounds;
    [SerializeField] private float _cubiePadding = 0.1f;

    [SerializeField] private float rotationDegreesPerSecond = 30f;
    [SerializeField] private float lookRotationDegreesPerSecond = 90f;

    private CubieGridMapper _cubieGridMapper;
    
    private Transform[,,] _cubies;

    private CubeLayer _layerToRotate;
    private float _degreesToRotateRemaining = 0f;
    
    public bool _isRotating = false;
    
    private Queue<(CubeLayer, float)> _rotationQueue = new();

    private InputAction _upAction;
    private InputAction _downAction;
    private InputAction _leftAction;
    private InputAction _rightAction;
    private InputAction _forwardAction;
    private InputAction _backAction;
    private InputAction _faceRotations;
    private InputAction _counterClockwiseAction;
    private InputAction _lookAction;
    private InputAction _resetAction;

    private CubeActions _cubeInputAsset;
    private CubeActions.GameplayActions _gameplayMap;


    void Awake()
    {
        _cubeInputAsset = new();
        _gameplayMap = _cubeInputAsset.Gameplay;
        _counterClockwiseAction = _gameplayMap.CounterClockwise;
        _lookAction = _gameplayMap.Look;
        _resetAction = _gameplayMap.Reset;
    }
    
    void Start()
    {
        _cubieBounds = cubiePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        Logger.LogFields(new {_cubieBounds.center, _cubieBounds.size});

        _cubieGridMapper = new CubieGridMapper(_cubeSize, _cubiePadding, _cubieBounds);
        SpawnCubies();

        var collider = GetComponent<BoxCollider>();
        collider.size = _cubieGridMapper.CubeSize();

        TryDequeRotation();
    }

    private void OnEnable()
    {
        _gameplayMap.Enable();
        _gameplayMap.Up.performed += OnUpPerformed;
        _gameplayMap.Down.performed += OnDownPerformed;
        _gameplayMap.Left.performed += OnLeftPerformed;
        _gameplayMap.Right.performed += OnRightPerformed;
        _gameplayMap.Front.performed += OnFrontPerformed;
        _gameplayMap.Back.performed += OnBackPerformed;
        _gameplayMap.Reset.performed += OnResetPerformed;
    }
    
    private void OnDisable()
    {
        _gameplayMap.Up.performed -= OnUpPerformed;
        _gameplayMap.Down.performed -= OnDownPerformed;
        _gameplayMap.Left.performed -= OnLeftPerformed;
        _gameplayMap.Right.performed -= OnRightPerformed;
        _gameplayMap.Front.performed -= OnFrontPerformed;
        _gameplayMap.Back.performed -= OnBackPerformed;
        _gameplayMap.Reset.performed -= OnResetPerformed;
        _gameplayMap.Disable();
    }



    private void OnDestroy()
    {
        _cubeInputAsset.Dispose();
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
                    // spawnPosition += transform.position;
                    GameObject cubieGo = Instantiate(cubiePrefab, transform.position, transform.rotation, transform);
                    cubieGo.transform.localPosition = spawnPosition;
                    cubieGo.name = $"Cubie ({x}, {y}, {z})";
                    _cubies[x, y, z] = cubieGo.transform;
                }
            }
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        ReadAndApplyLookInput();
        if (_isRotating)
        {
            // Logger.LogOnce($"UPDATE {Time.time}");
            // Debug.Log($"UPDATE {Time.time} {Time.deltaTime}");
            
            AnimateRotateLayer();
        }

    }
    
    // Should this method have a better name? Should it change transform?
    private void ReadAndApplyLookInput()
    {
        if (_lookAction.IsPressed())
        {
            var inputValue = _lookAction.ReadValue<Vector2>();
            transform.Rotate(Vector3.up, inputValue.x * lookRotationDegreesPerSecond * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, inputValue.y * lookRotationDegreesPerSecond * Time.deltaTime, Space.World);
        }
    }

    private void OnResetPerformed(InputAction.CallbackContext context)
    {
        Mouse.current.WarpCursorPosition(Vector2.zero);
        if (_isRotating) return;
        
        Debug.Log("OnReset()");
        for (int x = 0; x < _cubeSize; x++)
        {
            for (int y = 0; y < _cubeSize; y++)
            {
                for (int z = 0; z < _cubeSize; z++)
                {
                    _cubies[x, y, z].rotation = transform.rotation;
                }
            }
        }
    }

    private void OnUpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnUpPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.U, 90 * signMultiplier);
    }
    
    private void OnDownPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnDownPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.D, 90 * signMultiplier);
    }

    private void OnLeftPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnLeftPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.L, 90 * signMultiplier);
    }

    private void OnRightPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnRightPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.R, 90 * signMultiplier);
    }

    private void OnFrontPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnFrontPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.F, 90 * signMultiplier);
    }

    private void OnBackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"OnBackPerformed() {context}");
        int signMultiplier = _counterClockwiseAction.IsPressed() ? -1 : 1;
        QueueRotateLayer(CubeLayer.B, 90 * signMultiplier);
    }
    
    private void QueueRotateLayer(CubeLayer layer, float degrees)
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

        float degreesToRotateNow = Mathf.Min(Mathf.Abs(_degreesToRotateRemaining), Time.deltaTime * rotationDegreesPerSecond);
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

        switch ((int)layer / _cubeSize)
        {
            case 0 : axis = transform.right;
                f = (t) => t.right; break;
            case 1 : axis = transform.forward; f = (t) => t.forward; break;
            case 2 : axis = transform.up; f = (t) => t.up; break;
        }

        var dummyAxis = new GameObject("DummyAxis");
        var dummyPoint = new GameObject("DummyPoint");
        dummyAxis.transform.position = transform.position;
        dummyPoint.transform.position = transform.position;
        dummyAxis.transform.SetParent(transform);
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

