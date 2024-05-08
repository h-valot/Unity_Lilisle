using UnityEngine;

[CreateAssetMenu(fileName = "Tile_", menuName = "Data/Config/Tile")]
public class TileConfig : ScriptableObject
{
	[Header("Tile settings")]
	public string tileName;
	[TextArea] public string description;
	public Color borderColor;
	public Sprite icon;
	public int cost;
	public Tile3D tile;
}