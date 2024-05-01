using UnityEngine;

public class SnappingGrid : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private Camera _camera;

	private const float _CELL_SIZE = 1f;

    public Vector3 GetNearestPointOnGrid(Vector3 clickPoint) 
    {
        clickPoint -= transform.position;
        
        // x and z position based on the spacing size
        int xCount = Mathf.RoundToInt(clickPoint.x / _CELL_SIZE);
        int zCount = Mathf.RoundToInt(clickPoint.z / _CELL_SIZE);
        
        var output = new Vector3(xCount * _CELL_SIZE, 0, zCount * _CELL_SIZE);
        output += transform.position;
        return output;
    }
}