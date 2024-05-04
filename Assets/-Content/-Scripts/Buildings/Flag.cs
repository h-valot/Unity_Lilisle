using System.Collections.Generic;
using UnityEngine;

public class Flag : Tile3D
{
    [Header("FLAG")]

	[Header("Tweakable values")]
    [SerializeField] private FlagConfig _flagConfig;

	[Header("External references")]
    [SerializeField] private Ground _groundPrefab;
	[SerializeField] private RSO_GroundGrid _rsoGroundGrid;

	[Header("Debugging")]
    public List<Vector3Int> voidPositions;

	private Vector3Int _voidPosition;
	private Ground _newGround;

    public override void DoPlacementAction(Tile3D[] surroundHits, Ground belowTile) 
	{
		// Filling nearby void
        for (int x = -_flagConfig.range; x < _flagConfig.range + 1; x++)
		{
            for (int z = -_flagConfig.range; z < _flagConfig.range + 1; z++)
			{
				// Ignore corner
                if (x == -_flagConfig.range && z == -_flagConfig.range 
				|| x == -_flagConfig.range && z == _flagConfig.range 
				|| x == _flagConfig.range && z == -_flagConfig.range 
				|| x == _flagConfig.range && z == _flagConfig.range) 
				{
					continue;
                }

				// Get void position
				_voidPosition = new Vector3Int((int)gameObject.transform.position.x + x, -1, (int)gameObject.transform.position.z + z);

				// Ignore already placed ground
				if (_rsoGroundGrid.value[_voidPosition.x, _voidPosition.z]
				&& _rsoGroundGrid.value[_voidPosition.x, _voidPosition.z].isPlaced)
				{
					continue;
				}

				// Create new ground at void position
				_newGround = Instantiate(_groundPrefab, _voidPosition, Quaternion.identity, gameObject.transform);
				_newGround.transform.name = $"Ground_x{_newGround.transform.position.x}_z{_newGround.transform.position.z}";
				_rsoGroundGrid.value[_voidPosition.x, _voidPosition.z] = _newGround;
				_rsoGroundGrid.value[_voidPosition.x, _voidPosition.z].isPlaced = true;
			}
		}
    }
}