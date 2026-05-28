using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceIdentifier : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gizmoLength = 5f;
    [SerializeField] private Vector3 _worldDirectionMatchingCameraUp;
    [SerializeField] private Vector3Int _localDirectionMatchingCameraUp;
    [SerializeField] private Vector3 _worldDirectionMatchingCameraBack;
    [SerializeField] private Vector3Int _localDirectionMatchingCameraBack;
    
    void Update()
    {
        (_worldDirectionMatchingCameraUp, _localDirectionMatchingCameraUp) = IdentifyCubeDirectionMatching(cameraTransform.up);
        (_worldDirectionMatchingCameraBack, _localDirectionMatchingCameraBack) = IdentifyCubeDirectionMatching(-cameraTransform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _worldDirectionMatchingCameraUp * gizmoLength);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _worldDirectionMatchingCameraBack * gizmoLength);
    }

    private (Vector3, Vector3Int) IdentifyCubeDirectionMatching(Vector3 cameraDirection)
    {
        Vector3 worldDirectionMatchingCameraDirection = Vector3.zero;
        Vector3Int localDirectionMatchingCameraDirection = Vector3Int.zero;
        float maxAbsoluteDotProduct = 0f;
        
        var directionTuplets = new List<(Vector3, Vector3Int)>(){ (transform.up, Vector3Int.up), (transform.right, Vector3Int.right),
            (transform.forward, Vector3Int.forward) };
        foreach (var (worldDirection, localDirection) in directionTuplets)
        {
            float dotProduct = Vector3.Dot(worldDirection, cameraDirection);
            float absDotProduct = Mathf.Abs(dotProduct);
            if (absDotProduct > maxAbsoluteDotProduct)
            {
                worldDirectionMatchingCameraDirection = worldDirection;
                localDirectionMatchingCameraDirection = localDirection;
                maxAbsoluteDotProduct = absDotProduct;
                if (dotProduct < 0)
                {
                    worldDirectionMatchingCameraDirection *= -1;
                    localDirectionMatchingCameraDirection *= -1;
                }
            }
        }
        return (worldDirectionMatchingCameraDirection, localDirectionMatchingCameraDirection);
    }
}
