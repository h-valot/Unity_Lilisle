using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "Data/Config/Enemy")]
public class EnemyConfig : ScriptableObject
{
	public string enemyName;
	public string description;
    public float delay;
	public int health;
    public int speed;
	public int damage;
}