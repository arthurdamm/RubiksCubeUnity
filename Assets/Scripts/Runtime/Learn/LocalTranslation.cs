using UnityEngine;

public class LocalTranslation : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;

    private GameObject parent, child;

    void Awake()
    {
        
        
        parent = GameObject.CreatePrimitive(PrimitiveType.Cube);
        parent.GetComponent<Renderer>().material.color = Color.red;
        parent.name = "Parent";

        child = GameObject.CreatePrimitive(PrimitiveType.Cube);
        child.GetComponent<Renderer>().material.color = Color.green;
        child.name = "Child";
        
        
        child.transform.position = new Vector3(2, 0, 0);
        
        Debug.Log($"0 child position: {child.transform.position}");
        Debug.Log($"0 child localposition: {child.transform.localPosition}");
        
        child.transform.SetParent(parent.transform);
        
        Debug.Log($"1 child position: {child.transform.position}");
        Debug.Log($"1 child localposition: {child.transform.localPosition}");
        
        parent.transform.Rotate(Vector3.up, -90);
        
        Debug.Log($"2 child position: {child.transform.position}");
        Debug.Log($"2 child localposition: {child.transform.localPosition}");


        // empty1 = new GameObject("Dummy");
        // empty1.transform.position = parent.transform.position + new Vector3(0, .5f, 0);
        // empty1.transform.SetParent(parent.transform);

    }

    void Update()
    {
        parent.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        // empty1.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        child.transform.Rotate(xAngle, yAngle, zAngle, Space.World);

        child.transform.SetParent(null);
        Debug.Log($"3 child now position: {child.transform.position}");
        Debug.Log($"3 child localposition: {child.transform.localPosition}");
    }
}
