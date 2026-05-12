using UnityEngine;
using UnityEngine.UIElements;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubiePrefab;

    [SerializeField] private int _cubeSize = 3;
    [SerializeField] private Bounds _cubieBounds;
    [SerializeField] private float _cubiePadding = 0.5f;

    private CubieGridMapper _cubieGridMapper;
    
    private Transform[,,] _cubies;
    
    void Start()
    {
        _cubieBounds = cubiePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        Logger.LogFields(new {_cubieBounds.center, _cubieBounds.size});

        _cubieGridMapper = new CubieGridMapper(_cubeSize, _cubiePadding, _cubieBounds);
        SpawnCubies();
        // RotateLayer(CubeLayer.L, 10);
        // RotateLayer(CubeLayer.R, 10);
        // RotateLayer(CubeLayer.F, 90);
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

    private void RotateLayer(CubeLayer layer, float degrees)
    {
        int xStart = 0, xStop = _cubeSize - 1, yStart = 0, yStop = _cubeSize - 1, zStart = 0, zStop = _cubeSize - 1;

        Transform[,,] cubiesCopy = (Transform[,,])_cubies.Clone();
        
        switch (layer)
        {
            case CubeLayer.L: xStart = xStop = 0; break;
            case CubeLayer.X: xStart = xStop = 1; break;
            case CubeLayer.R: xStart = xStop = 2; break;
            case CubeLayer.D: yStart = yStop = 0; break;
            case CubeLayer.Z: yStart = yStop = 1; break;
            case CubeLayer.U: yStart = yStop = 2; break;
            case CubeLayer.F: zStart = zStop = 0; break;
            case CubeLayer.Y: zStart = zStop = 1; break;
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
    D,
    Z,
    U,
    F,
    Y,
    B
}
