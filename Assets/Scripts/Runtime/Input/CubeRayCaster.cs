using UnityEngine;

/*
 * CubeRayCaster
 * Takes DragResult and ray casts its onto the Cube
 * 
 */
public class CubeRayCaster
{
    private Camera _camera;

    public CubeRayCaster(Camera camera)
    {
        _camera = camera;
    }

    public void CastDrag(DragResult drag)
    {
        RaycastHit startHit, stopHit;

        if (!Raycast(drag.Start, out startHit) || !Raycast(drag.Stop, out stopHit))
        {
            return;
        }
        Debug.DrawRay(startHit.point, (stopHit.point - startHit.point).normalized * 5f, Color.indigo, 60f);
        Debug.DrawRay(startHit.point, startHit.normal * .5f, Color.red, 60f);
        Debug.DrawRay(stopHit.point, stopHit.normal * .5f, Color.red, 60f);
    }
    
    private bool Raycast(Vector2 screenPoint, out RaycastHit hit)
    {
        Ray ray = _camera.ScreenPointToRay(screenPoint);
        if (!Physics.Raycast(ray, out hit))
        {
            return false;
        }
        Debug.Log($"Raycast() {screenPoint} to {hit.point} / {hit.normal}");
        return true;
    }
    
    
}