using UnityEngine;

public class TilePlacer : MonoBehaviour 
{
    [Header("Tweakable values")] 
    [SerializeField] private int _gridSize;

    [Header("References")] 
    [SerializeField] private GameObject _cityGround;
    [SerializeField] private Transform _gameObjectsContainer;
    [SerializeField] private RSE_SetCursor _rseSetCursor;
    [SerializeField] private RSE_TilePlaced _rseTilePlaced;
	[SerializeField] private RSE_TilePlacementFailed _rseTilePlacementFailed;
    [SerializeField] private RSO_GroundGrid _rsoGroundGrid;
    [SerializeField] private RSO_TileGrid _rsoTileGrid;

	private const int _SURROUND_TILES_LENGTH = 9;
	private const float _CELL_SIZE = 1f;

    private Camera _camera;
    private Tile3D _cursor = null;
	private Vector3 _lastFrameCursorPos;
	private Vector3Int _roundedCursorPos;
	private Tile3D[] _surroundTiles;
	private Ground _belowGround;

    private void Awake() 
    {
        _camera = Camera.main;
    }

	public void Initialize()
	{
		_rsoTileGrid.value = new Tile3D[_gridSize, _gridSize];
		_rsoGroundGrid.value = new Ground[_gridSize, _gridSize];
		_gameObjectsContainer.position = new Vector3(_gridSize / 2, 0, _gridSize / 2);

		foreach (Transform child in transform)
		{
			if (child.TryGetComponent<Tile3D>(out var tile))
			{
				_rsoTileGrid.value[
					Mathf.RoundToInt(child.position.x), 
					Mathf.RoundToInt(child.position.z)
				] = tile;
			}

			if (child.TryGetComponent<Ground>(out var ground))
			{
				_rsoGroundGrid.value[
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
        	_cursor.transform.position = GetNearestPointOnGrid(hitInfo.point);
		}

		// Optimization: avoid doing same calculation endlessly
		if (_cursor.transform.position != _lastFrameCursorPos)
		{
			_lastFrameCursorPos = _cursor.transform.position;
			VerifyPlacement();
		}
	}

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

				if (_rsoTileGrid.value[x, z] != null)
				{
					_surroundTiles[surroundTileCount] = _rsoTileGrid.value[x, z];
				}
				localZ++;
			}
			localX++;
			localZ = -1;
		}

		_belowGround = _rsoGroundGrid.value[_roundedCursorPos.x, _roundedCursorPos.z];
		
		// Check out the tile placement verification
		_cursor.VerifyPlacement(_surroundTiles, _belowGround);
	}

	private void HandleInputs()
	{
		// Input: place a tile at the mouse over position on click
		if (Input.GetMouseButtonDown(0)) 
		{
			if (_cursor.canBePlaced)
			{
				PlaceTile(_surroundTiles, _belowGround);
			}
			else 
			{
				_rseTilePlacementFailed.Call();
			}

		}
	
		// Input: rotate this object
		if (Input.GetMouseButtonDown(1))
		{
			_cursor.transform.eulerAngles += new Vector3(0f, 90f, 0f);
			VerifyPlacement();
		}
	}

    private void PlaceTile(Tile3D[] surroundTiles, Ground belowGround) 
    {        
        // Instantiate the cursor as a tile
        Tile3D newTile = Instantiate(_cursor);
        newTile.transform.position = _cursor.transform.position;
        newTile.transform.name = $"{_cursor.transform.name}_x{_cursor.transform.position.x}_z{_cursor.transform.position.z}";
        newTile.transform.parent = transform;
        newTile.isPlaced = true;

		_rsoTileGrid.value[
			Mathf.RoundToInt(newTile.transform.position.x), 
			Mathf.RoundToInt(newTile.transform.position.z)
		] = newTile;   

        // Replace dirt mesh by city mesh
		// belowGround.SetMesh(GroundType.CITY);

        newTile.DoPlacementAction(surroundTiles, belowGround);
		VerifyPlacement();
		_rseTilePlaced.Call();
    }
    
    public void SetCursor(Tile3D newCursor) 
    {
        // Destroy former cursor
        if (_cursor) 
        {
            Destroy(_cursor.gameObject);
        }
        
		if (newCursor == null)
		{
			_cursor = null;
			return;
		}

        // Instantiate the new one
        _cursor = Instantiate(newCursor);
        _cursor.transform.name = newCursor.transform.name;
        _cursor.transform.parent = gameObject.transform;
    }

	private void OnEnable()
	{
		_rseSetCursor.action += SetCursor;
	}

	private void OnDisable()
	{
		_rseSetCursor.action -= SetCursor;
	}
}