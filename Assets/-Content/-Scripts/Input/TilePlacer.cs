using UnityEngine;

public class TilePlacer : MonoBehaviour 
{
    [Header("References")] 
    [SerializeField] private SnappingGrid _snappingGrid;
    [SerializeField] private GameObject _cityGround;
    [SerializeField] private Transform _gameObjectsContainer;

    [Header("Tweakable values")] 
    [SerializeField] private int _gridSize;

    private Camera _camera;
    private Tile3D _cursor = null;
	private Vector3 _lastFrameCursorPos;
	private Vector3Int _roundedCursorPos;

	private const int _SURROUND_TILES_LENGTH = 9;

	private Tile3D[,] _tileGrid;
	private Ground[,] _groundGrid;
	private Tile3D[] _surroundTiles;
	private Ground _belowGround;

    private void Awake() 
    {
        _camera = Camera.main;
    }

	public void Initialize()
	{
		_tileGrid = new Tile3D[_gridSize, _gridSize];
		_groundGrid = new Ground[_gridSize, _gridSize];
		_gameObjectsContainer.position = new Vector3(_gridSize / 2, 0, _gridSize / 2);

		foreach (Transform child in transform)
		{
			if (child.TryGetComponent<Tile3D>(out var tile))
			{
				_tileGrid[
					Mathf.RoundToInt(child.position.x), 
					Mathf.RoundToInt(child.position.z)
				] = tile;
			}

			if (child.TryGetComponent<Ground>(out var ground))
			{
				_groundGrid[
					Mathf.RoundToInt(child.position.x), 
					Mathf.RoundToInt(child.position.z)
				] = ground;
			}
        }
	}

    private void Update() 
    {
        // Null exceptions
        if (!_camera
        || _cursor == null) 
        {
            return;
        }

		HandleTilePlacement();
		HandleInputs();
    }

	private void HandleTilePlacement()
	{
        // Get mouse over position
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo)) 
        {
        	_cursor.transform.position = _snappingGrid.GetNearestPointOnGrid(hitInfo.point);
		}

		// Optimization: avoid doing same calculation endlessly
		if (_cursor.transform.position == _lastFrameCursorPos)
		{
			return;
		}
		_lastFrameCursorPos = _cursor.transform.position;

		VerifyPlacement();
	}
		
	private void VerifyPlacement()
	{
		_roundedCursorPos = new Vector3Int(
			Mathf.RoundToInt(_cursor.transform.position.x),
			Mathf.RoundToInt(_cursor.transform.position.y),
			Mathf.RoundToInt(_cursor.transform.position.z)
		);

		int surroundTileCount = 0;
		int localX = -1, localZ = -1;
		_surroundTiles = new Tile3D[_SURROUND_TILES_LENGTH];
		for (int x = _roundedCursorPos.x - 1 ; x < _roundedCursorPos.x + 2; x++)
		{
			for (int z = _roundedCursorPos.z - 1; z < _roundedCursorPos.z + 2; z++)
			{
				surroundTileCount++;

				// Avoiding corners
				if (Mathf.Abs(localX) + Mathf.Abs(localZ) == 2)
				{
					localZ++;
					continue;
				}

				if (_tileGrid[x, z] != null)
				{
					_surroundTiles[surroundTileCount] = _tileGrid[x, z];
				}
				localZ++;
			}
			localX++;
			localZ = -1;
		}

		_belowGround = _groundGrid[_roundedCursorPos.x, _roundedCursorPos.z];
		
		// Check out the tile placement verification
		_cursor.VerifyPlacement(_surroundTiles, _belowGround);
	}

	private void HandleInputs()
	{
		// Input: place a tile at the mouse over position on click
		if (Input.GetMouseButtonDown(0)
		&& _cursor.canBePlaced) 
		{
			PlaceTile(_surroundTiles, _belowGround);
		}
	
		// Input: rotate this object
		if (Input.GetMouseButtonDown(1))
		{
			_cursor.transform.eulerAngles += new Vector3(0f, 90f, 0f);
		}
	}

    private void PlaceTile(Tile3D[] surroundTiles, Ground belowGround) 
    {        
        // Instantiate the cursor as a tile
        Tile3D NewTile = Instantiate(_cursor);
        NewTile.transform.position = _cursor.transform.position;
        NewTile.transform.name = $"{_cursor.transform.name}_x{_cursor.transform.position.x}_z{_cursor.transform.position.z}";
        NewTile.transform.parent = transform;
        NewTile.isPlaced = true;     

		_tileGrid[
			Mathf.RoundToInt(NewTile.transform.position.x), 
			Mathf.RoundToInt(NewTile.transform.position.z)
		] = NewTile;   

        // Replace dirt mesh by city mesh
		// belowGround.SetMesh(GroundType.CITY);

        NewTile.DoPlacementAction(surroundTiles, belowGround, _cityGround);
		VerifyPlacement();
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