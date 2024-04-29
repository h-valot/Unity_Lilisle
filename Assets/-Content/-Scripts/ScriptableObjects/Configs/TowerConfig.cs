using UnityEngine;

[CreateAssetMenu(fileName = "Tower_", menuName = "Data/Config/Tower")]
public class TowerConfig : ScriptableObject
{
	public string towerName;
	public string description;
	public float atkRange;
    public float fireRate;
    public int damage;
}