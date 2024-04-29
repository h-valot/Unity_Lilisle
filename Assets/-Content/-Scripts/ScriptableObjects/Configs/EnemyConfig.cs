using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Data/Config/Enemy")]
public class EnemyConfig : ScriptableObject
{
    public int speed;
    public float delay;
	public int damage;
}