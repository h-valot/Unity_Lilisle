using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Data/Config/Wave")]
public class WaveConfig : ScriptableObject
{
    public EnemyConfig[] enemies;
}