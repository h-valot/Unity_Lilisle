using UnityEngine;

public class Tile3D : MonoBehaviour 
{
    [Header("TILE 3D")] 
    [Header("Color placement")] 
    [SerializeField] private Renderer _meshRenderer;
    [SerializeField] private Material _greenMaterial, _redMaterial, _voidMaterial;

    [Header("Water combining")]
    [SerializeField] private bool _canBeCombineWithWater;
    [SerializeField] private int _waterPositionAngle;
    [SerializeField] private float _chanceToSpawnWithWater = .3f;

	[Header("Debugging")]
    public bool canBePlaced;
    public bool isPlaced;
    
    protected void Start() 
    {
        SetUpRendered();
        HandleWater();
    }
    
    protected void Update() 
    {
        SetColor();
    }

    protected void SetUpRendered() 
    {
        _meshRenderer.enabled = true;
        _meshRenderer.sharedMaterial = _voidMaterial;
    }
    
    protected void HandleWater() 
    {
        float rnd = Random.Range(0f, 1f);

        if (rnd <= _chanceToSpawnWithWater)
        {
            _canBeCombineWithWater = true;
        }
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
		if (belowTile != null
		&& belowTile.type != GroundType.WATER)
		{
			grounded = true;
		}

        if (surroundTileCount > 0 
        && grounded
		&& !overlapping)
        {
            canBePlaced = true;
        }
    }

    public virtual void DoPlacementAction(Tile3D[] surroundTiles, Ground belowGround) 
    {
		// Do nothing for the moment
    }
}