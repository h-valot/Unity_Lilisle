using UnityEngine;

[CreateAssetMenu(fileName = "Tower_", menuName = "Data/Config/Tower")]
public class TowerConfig : TileConfig
{
	[Header("Tower settings")]
	public float atkRange;
    public float fireRate;
    public int damage;
}