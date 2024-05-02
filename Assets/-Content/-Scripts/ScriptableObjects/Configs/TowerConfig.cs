using UnityEngine;

[CreateAssetMenu(fileName = "Tower_", menuName = "Data/Config/Tower")]
public class TowerConfig : TileConfig
{
	public float atkRange;
    public float fireRate;
    public int damage;
}