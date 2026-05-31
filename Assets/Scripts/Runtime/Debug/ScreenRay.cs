using UnityEngine;

public class ScreenRay : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayLength = 20f;
    [SerializeField] private Vector3 position;
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        position = new Vector3(Screen.width / 4, Screen.height / 3);
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.blueViolet);
        Debug.DrawLine(Vector3.zero, Vector3.right * rayLength, Color.brown, 60f);
        Vector3 start = ray.origin + Vector3.up * 3f;
        Debug.DrawLine(start, start + ray.direction * rayLength, Color.black);
    }
}
