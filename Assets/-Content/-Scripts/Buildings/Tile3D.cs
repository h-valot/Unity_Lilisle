using UnityEngine;

public class Tile3D : MonoBehaviour 
{
    [Header("TILE 3D")] 
    [Header("Color placement")] 
    [SerializeField] private Renderer _meshRenderer;
    [SerializeField] private Material _greenMaterial, _redMaterial, _voidMaterial;
    [SerializeField] private Transform _raycastPoint;

	[Header("Debugging")]
    public bool canBePlaced;
    public bool isPlaced;
    
	private Ray _ray;
	private RaycastHit[] _hits;
	private bool _pinHits;

    private void Start() 
    {
        SetUpRendered();
		TileStart();
    }

	protected virtual void TileStart()
	{
		// Do nothing in the parent
	}
    
    private void Update() 
    {
        SetColor();
		TileUpdate();
    }

	protected virtual void TileUpdate()
	{
		// Do nothing in the parent
	}

    protected void SetUpRendered() 
    {
        _meshRenderer.enabled = true;
        _meshRenderer.sharedMaterial = _voidMaterial;
    }

    protected void SetColor() 
    {
        // If the tile is already placed, disable its color
        if (isPlaced) 
        {
            _meshRenderer.sharedMaterial = _voidMaterial;
            return;
        }

        // Set the color green or red depending if the tile can be placed on the scene
        _meshRenderer.sharedMaterial = canBePlaced ? _greenMaterial : _redMaterial;
    }
    
    public virtual void VerifyPlacement(Tile3D[] surroundTiles, Ground belowTile) 
    {
        int surroundTileCount = 0;
        bool overlapping = false;
        bool grounded = false;
        canBePlaced = false;
        
        foreach (Tile3D tile in surroundTiles)
        {
			if (tile == null
			|| tile == this)
			{
				continue;
			}
			
			surroundTileCount++;

			if (tile.transform.position == transform.position)
			{
				overlapping = true;
			}
        }
        
        // Check if there a ground below the building
		if (belowTile != null)
		{
			grounded = true;
		}

        if (surroundTileCount > 0 
        && grounded
		&& !overlapping
		&& !IsInterruptingRoad())
        {
            canBePlaced = true;
        }
    }

	private bool IsInterruptingRoad()
	{
		_ray = new Ray(_raycastPoint.position, (transform.position - _raycastPoint.position).normalized);
		_hits = new RaycastHit[5];
		Physics.RaycastNonAlloc(_ray, _hits);

		if (_hits.Length <= 0)
		{
			return false;
		}

		_pinHits = false;
		for (int i = 0; i < _hits.Length; i++)
		{
			if (_hits[i].collider == null)
			{
				continue;
			}

			if (!_hits[i].collider.TryGetComponent<Pin>(out var pin))
			{
				continue;
			}

			_pinHits = true;
		}

		return _pinHits;
	}

    public virtual void DoPlacementAction(Tile3D[] surroundTiles, Ground belowGround) 
    {
		// Do nothing in the parent
    }
}