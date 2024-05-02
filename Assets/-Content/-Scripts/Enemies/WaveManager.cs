using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveConfig[] _waves;
	[SerializeField] private EnemyPool _enemyPool;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private RSO_GameState _rsoGameState;

	private const float _WAVE_DELAY = 0.2f;

	public void Initialize()
	{
		_rsoCurrentWave.value = -1;
	}

	public void PlayNextWave()
	{
		if (_rsoGameState.value == GameState.WAVE)
		{
			return;
		}

		_rsoCurrentWave.value++;
		if (_rsoCurrentWave.value > _waves.Length)
		{
			return;
		}

        StartCoroutine(HandleWaveWithDelay());
	}

    private IEnumerator HandleWaveWithDelay()
    {
		_rsoGameState.value = GameState.WAVE;
		
        for (int i = 0; i < _waves[_rsoCurrentWave.value].enemies.Length; i++)
        {
            Enemy newEnemy = _enemyPool.Get();
            newEnemy.Initialize(_waves[_rsoCurrentWave.value].enemies[i]);
            yield return new WaitForSeconds(_waves[_rsoCurrentWave.value].enemies[i].delay);
        }

		while (_enemyPool.IsEnemiesAlived())
		{
            yield return new WaitForSeconds(_WAVE_DELAY);
		}
		
		_rsoGameState.value = GameState.REWARD;
    }
}