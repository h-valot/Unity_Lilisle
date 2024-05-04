using UnityEngine;

public class Road : Tile3D
{
	[Header("ROAD")]
	[Header("References")]
	[SerializeField] private Connector _connector;
    [SerializeField] private Waypoint _waypoint;

    public override void VerifyPlacement(Tile3D[] surroundTiles, Ground belowTile) 
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
		&& _connector.IsLinked())
        {
            canBePlaced = true;
        }
    }
	
    public override void DoPlacementAction(Tile3D[] surroundTiles, Ground belowGround) 
    {
		PlacePath();
    }

	public void PlacePath()
	{
		_waypoint.Place();
	}
}