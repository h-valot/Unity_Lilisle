using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveConfig[] _waves;
	[SerializeField] private EnemyPool _enemyPool;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;

	private bool _spawning;

	public void Initialize()
	{
		_rsoCurrentWave.value = -1;
	}

	public void PlayNextWave()
	{
		if (_spawning)
		{
			return;
		}

		_rsoCurrentWave.value++;
		if (_rsoCurrentWave.value > _waves.Length)
		{
			return;
		}

        StartCoroutine(SpawnWaveWithDelay());
	}

    private IEnumerator SpawnWaveWithDelay()
    {
		_spawning = true;
        for (int i = 0; i < _waves[_rsoCurrentWave.value].enemies.Length; i++)
        {
            Enemy newEnemy = _enemyPool.Get();
            newEnemy.Initialize(_waves[_rsoCurrentWave.value].enemies[i]);
            yield return new WaitForSeconds(_waves[_rsoCurrentWave.value].enemies[i].delay);
        }
		_spawning = false;
    }
}