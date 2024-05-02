using UnityEngine;

[CreateAssetMenu(fileName = "Tile_", menuName = "Data/Config/Tile")]
public class TileConfig : ScriptableObject
{
	public string tileName;
	public string description;
	public Sprite icon;
	public int cost;
}