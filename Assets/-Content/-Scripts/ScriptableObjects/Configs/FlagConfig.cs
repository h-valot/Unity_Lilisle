using UnityEngine;

[CreateAssetMenu(fileName = "Flag_", menuName = "Data/Config/Flag")]
public class FlagConfig : TileConfig
{
	[Header("Flag settings")]
	public int range;
}