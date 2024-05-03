using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "Data/Config/Waves")]
public class WavesConfig : ScriptableObject
{
	public WaveConfig[] waves;
}