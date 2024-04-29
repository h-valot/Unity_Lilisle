using UnityEngine;

public class Tile3D : MonoBehaviour 
{
    [Header("Placement")] 
    public bool canBePlaced;
    public bool isPlaced;
    
    [Header("Color placement")] 
    public Material[] materials;
    private Renderer meshRenderer;
    public GameObject color;

    [Header("Water combining")]
    public bool canBeCombineWithWater;
    public int waterPositionAngle;
    public float chanceToSpawnWithWater = .3f;
    
    protected void Start() 
    {
        SetUpRendered();
        HandleWater();
    }
    
    protected void Update() 
    {
        SetColor();
    }

    private void SetUpRendered() 
    {
        meshRenderer = color.GetComponent<Renderer>();
        meshRenderer.enabled = true;
        meshRenderer.sharedMaterial = materials[0];
    }
    
    private void HandleWater() 
    {
        float rnd = UnityEngine.Random.Range(0f, 1f);

        if (rnd <= chanceToSpawnWithWater)
        {
            canBeCombineWithWater = true;
        }
    }

    private void SetColor() 
    {
        // If the tile is already placed, disable its color
        if (isPlaced) 
        {
            meshRenderer.sharedMaterial = materials[0];
            return;
        }

        // Set the color green or red depending if the tile can be placed on the scene
        meshRenderer.sharedMaterial = canBePlaced ? materials[1] : materials[2];
    }
    
    public virtual void VerifyPlacement(RaycastHit[] surroundHits, RaycastHit[] belowHits) 
    {
        // Initialize method variable
        int surroundBuildingCount = 0;
        bool buildingOverlapping = false;
        bool groundBelow = false;
        
        // Check if there is any tile nearby
		if (surroundHits.Length <= 0)
		{
            canBePlaced = false;
		}

        foreach (RaycastHit hit in surroundHits)
        {
            if (hit.collider.TryGetComponent<Tile3D>(out Tile3D building)) {
                
                // Ignore itself
                if (building == this) 
                {
                    continue;
                }
                
                // Check if there is a tile next to this one
                if (building.transform.position == transform.position + -1*transform.forward ||
                    building.transform.position == transform.position + transform.forward ||
                    building.transform.position == transform.position + -1*transform.right ||
                    building.transform.position == transform.position + transform.right)
                {
                    surroundBuildingCount++;
                }

                // Check if there is a tile into this one
                if (building.transform.position == transform.position)
                {
                    buildingOverlapping = true;
                }
            }
        }
        
        // Check if there a ground below the building
        foreach (RaycastHit hit in belowHits)
        {
            if (hit.collider.TryGetComponent<Ground>(out Ground ground))
            {
                if (ground.type != GroundType.WATER)
                {
                    groundBelow = true;
                }
            }
        }

        // If there is at least one tile nearby AND a ground below it, the placement is possible
        if (surroundBuildingCount >= 1 
        && groundBelow)
        {
            canBePlaced = true;
        }
        
        // If there is no tile nearby OR a tile onto this one OR no ground below it, the placement is impossible
        if (surroundBuildingCount == 0 
        || buildingOverlapping 
        || !groundBelow)
        {
            canBePlaced = false;
        }
    }

    public virtual void DoPlacementAction(RaycastHit[] surroundHits, RaycastHit[] belowHits, GameObject prefabReplacement) 
    {
        // Replace dirt ground by city
        foreach (RaycastHit hit in belowHits)
        {
            if (hit.collider.TryGetComponent<Ground>(out Ground ground))
            {
                if (ground.type == GroundType.WATER) 
				{
					continue;
                }

				// Get the dirt position before destroying it
				Vector3 dirtPosition = ground.transform.position;
				Destroy(ground.gameObject);
				
				// Instantiate the city ground under the tile
				Instantiate(prefabReplacement, dirtPosition, Quaternion.identity, gameObject.transform);
            }
        }
    }
}