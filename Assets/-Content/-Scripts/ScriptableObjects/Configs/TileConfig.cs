using UnityEngine;

[CreateAssetMenu(fileName = "Tile_", menuName = "Data/Config/Tile")]
public class TileConfig : ScriptableObject
{
	[Header("Tile settings")]
	public string tileName;
	public string description;
	public Sprite icon;
	public int cost;
	public Tile3D tile;
}