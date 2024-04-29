using UnityEngine;
using Color = UnityEngine.Color;

public class SnappingGrid : MonoBehaviour 
{
    [Header("Tweakable values")]
    [SerializeField] private float _cellSize = 1f;

    [Header("References")]
    [SerializeField] private Camera _camera;

    public Vector3 GetNearestPointOnGrid(Vector3 clickPoint) 
    {
        clickPoint -= transform.position;
        
        // x and z position based on the spacing size
        int xCount = Mathf.RoundToInt(clickPoint.x / _cellSize);
        int zCount = Mathf.RoundToInt(clickPoint.z / _cellSize);
        
        var output = new Vector3(xCount * _cellSize, 0, zCount * _cellSize);
        output += transform.position;
        return output;
    }

    public void OnDrawGizmos() 
    {
        // Set gizmo color
        Gizmos.color = Color.yellow;
           
        // Get mouse over position
        var lookingPoint = new Vector3(0, 0, 0);
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo)) 
        { 
            lookingPoint = GetNearestPointOnGrid(hitInfo.point);;
        }

        // Draw points
        for (float x = lookingPoint.x - 1; x < lookingPoint.x + 2; x++)
        {
            for (float z = lookingPoint.z - 1; z < lookingPoint.z + 2; z++)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
    }
}