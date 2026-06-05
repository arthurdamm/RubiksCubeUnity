using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;


    private void OnValidate()
    {
        if (target)
        {
            transform.LookAt(target);
            UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(this);
        }
    }
}
