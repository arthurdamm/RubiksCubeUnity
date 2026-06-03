using System;
using UnityEngine;

public class CubeBuilder : MonoBehaviour
{
    [SerializeField] private GameObject cubiePrefab;

    [SerializeField] private int cubeSize = 3;
    [SerializeField] private float cubiePadding = 0.1f;

    [SerializeField] private float rotationDegreesPerSecond = 30f;
    
    public CubeModel BuildModel()
    {
        var cubieBounds = cubiePrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        Logger.LogFields(new {cubieBounds.center, cubieBounds.size});

        var cubeLayout = new CubeLayout(cubeSize, cubiePadding, cubieBounds);
        var cubieGridMapper = new CubieGridMapper(cubeLayout);
        var cubies = SpawnCubies(cubeLayout, cubieGridMapper);

        return new CubeModel(cubies, cubieGridMapper, transform, rotationDegreesPerSecond);
    }
    
    private Transform[,,] SpawnCubies(CubeLayout cubeLayout, CubieGridMapper cubieGridMapper)
    {
        Transform[,,] cubies = new Transform[cubeLayout.CubeSize, cubeLayout.CubeSize, cubeLayout.CubeSize];

        for (int x = 0; x < cubeLayout.CubeSize; x++)
        {
            for (int y = 0; y < cubeLayout.CubeSize; y++)
            {
                for (int z = 0; z < cubeLayout.CubeSize; z++)
                {
                    Vector3 spawnPosition = cubieGridMapper.GridIndexToLocalPosition(new Vector3Int(x, y, z));
                    Debug.Log(spawnPosition);
                    // spawnPosition += transform.position;
                    GameObject cubieGo = Instantiate(cubiePrefab, transform.position, transform.rotation, transform);
                    cubieGo.transform.localPosition = spawnPosition;
                    cubieGo.name = $"Cubie ({x}, {y}, {z})";
                    cubies[x, y, z] = cubieGo.transform;
                }
            }
        }

        return cubies;
    }

}
