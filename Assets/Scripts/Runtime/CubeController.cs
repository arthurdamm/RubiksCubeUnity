using UnityEngine;
using UnityEngine.UIElements;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubiePrefab;

    private int _cubeSize = 3;
    private Bounds _cubieBounds;
    private float _cubiePadding = 0.01f;
    
    private Transform[,,] _cubies;
    
    void Start()
    {
        _cubieBounds = cubiePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        SpawnCubies();
        RotateLayer(CubeLayer.L, 90);
        RotateLayer(CubeLayer.F, 45);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    Vector3 spawnPosition = new Vector3(
                        x * (_cubieBounds.size.x + _cubiePadding),
                        y * (_cubieBounds.size.y + _cubiePadding),
                        z * (_cubieBounds.size.z + _cubiePadding));
                    Debug.Log(spawnPosition);
                    spawnPosition += transform.position;
                    GameObject cubieGo = Instantiate(cubiePrefab, spawnPosition, Quaternion.identity, transform);
                    cubieGo.name = $"Cubie ({x}, {y}, {z})";
                    _cubies[x, y, z] = cubieGo.transform;
                    
                }
            }
        }
    }

    private void RotateLayer(CubeLayer layer, float degrees)
    {
        int xStart = 0, xStop = _cubeSize - 1, yStart = 0, yStop = _cubeSize - 1, zStart = 0, zStop = _cubeSize - 1;
        
        switch (layer)
        {
            case CubeLayer.L: xStart = xStop = 0; break;
            case CubeLayer.X: xStart = xStop = 1; break;
            case CubeLayer.R: xStart = xStop = 2; break;
            case CubeLayer.D: yStart = yStop = 0; break;
            case CubeLayer.Y: yStart = yStop = 1; break;
            case CubeLayer.U: yStart = yStop = 2; break;
            case CubeLayer.F: zStart = zStop = 0; break;
            case CubeLayer.Z: zStart = zStop = 1; break;
            case CubeLayer.B: zStart = zStop = 2; break;
        }

        Vector3 center = _cubies[(xStart + xStop) / 2, (yStart + yStop) / 2, (zStart + zStop) / 2].position;
        Vector3 axis = Vector3.up;

        switch ((int)layer / _cubeSize)
        {
            case 0 : axis = Vector3.right; break;
            case 1 : axis = Vector3.up; break;
            case 2 : axis = Vector3.forward; break;
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
}

enum CubeLayer
{
    L,
    X,
    R,
    D,
    Y,
    U,
    F,
    Z,
    B
}