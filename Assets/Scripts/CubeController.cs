using UnityEngine;

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
}
