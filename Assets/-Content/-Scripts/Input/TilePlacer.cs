using UnityEngine;

public class TilesPlacer : MonoBehaviour 
{
    [Header("References")] 
    [SerializeField] private SnappingGrid _grid;
    [SerializeField] private GameObject _cityGround;

    private Camera _camera;
    private Tile3D _cursor;

    private void Awake() 
    {
        _camera = Camera.main;
    }

    private void Update() 
    {
        // Null exceptions
        if (!_camera
        || !_cursor) 
        {
            return;
        }

        // Get mouse over position
        var ray = _camera.ScreenPointToRay(Input.mousePosition); 
        if (Physics.Raycast(ray, out var hitInfo)) 
        {
            _cursor.transform.position = _grid.GetNearestPointOnGrid(hitInfo.point);

            RaycastHit[] surroundHits = Physics.BoxCastAll(
				_cursor.transform.position, 
				new Vector3(1.25f, 0.5f, 1.25f), 
				new Vector3(0, 0.25f, 0)
			);

            RaycastHit[] belowHits = Physics.BoxCastAll(
				_cursor.transform.position,
				new Vector3(0.25f, 0.25f, 0.25f), 
				new Vector3(0, -0.25f, 0)
			);
            
            // Check out the tile placement verification
            _cursor.VerifyPlacement(surroundHits, belowHits);
        
            // Input: place a tile at the mouse over position on click
            if (Input.GetMouseButtonDown(0)
            && _cursor.canBePlaced) 
            {
                PlaceObject(surroundHits, belowHits);
            }
        
            // Input: rotate this object
            if (Input.GetMouseButtonDown(1))
            {
                _cursor.transform.eulerAngles += new Vector3(0f, 90f, 0f);
            }
        }
    }

    private void PlaceObject(RaycastHit[] surroundHits, RaycastHit[] belowHits) 
    {
        // Tile placement verification
        if (!_cursor.canBePlaced)
        {
            return;
        }
        
        // Instantiate the cursor as a tile
        Tile3D tilePlaced = Instantiate(_cursor);
        tilePlaced.transform.position = _cursor.transform.position;
        tilePlaced.transform.name = $"{_cursor.transform.name}_x{_cursor.transform.position.x}_y{_cursor.transform.position.y}_z{_cursor.transform.position.z}";
        tilePlaced.transform.parent = gameObject.transform;
        tilePlaced.isPlaced = true;

        if (tilePlaced.TryGetComponent<Path>(out Path newPath) 
        && _cursor.TryGetComponent<Path>(out Path cursor))
        {
            // newPath.commonLinks = cursor.commonLinks;
        }
        
        tilePlaced.DoPlacementAction(surroundHits, belowHits, _cityGround);
    }
    
    public void SetCursor(Tile3D newCursor) 
    {
        // Destroy former cursor
        if (_cursor) 
        {
            Destroy(_cursor.gameObject);
        }
        
        // Instantiate the new one
        _cursor = Instantiate(newCursor);
        _cursor.transform.name = newCursor.transform.name;
        _cursor.transform.parent = gameObject.transform;
    }
}