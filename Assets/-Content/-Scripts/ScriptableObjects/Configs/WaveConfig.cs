using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Data/Config/Wave")]
public class WaveConfig : ScriptableObject
{
    public EnemyConfig[] enemies;
}