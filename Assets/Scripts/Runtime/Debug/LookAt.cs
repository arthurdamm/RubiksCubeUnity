using UnityEngine;
using UnityEditor;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (target)
        {
            transform.LookAt(target);
            EditorApplication.delayCall += () => DestroyImmediate(this);
        }
    }
#endif
    
}
