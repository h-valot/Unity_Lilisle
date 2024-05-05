using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WavesConfig _wavesConfig;
	[SerializeField] private EnemyPool _enemyPool;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSE_PlayNextWave _rsePlayNextWave;

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
		if (_rsoCurrentWave.value > _wavesConfig.waves.Length)
		{
			return;
		}

        StartCoroutine(HandleWaveWithDelay());
	}

    private IEnumerator HandleWaveWithDelay()
    {
		_rsoGameState.value = GameState.WAVE;
		
        for (int i = 0; i < _wavesConfig.waves[_rsoCurrentWave.value].enemies.Length; i++)
        {
            yield return new WaitForSeconds(_wavesConfig.waves[_rsoCurrentWave.value].enemies[i].delay);
            Enemy newEnemy = _enemyPool.Get();
            newEnemy.Initialize(_wavesConfig.waves[_rsoCurrentWave.value].enemies[i]);
        }

		while (_enemyPool.IsEnemiesAlived())
		{
            yield return new WaitForSeconds(_WAVE_DELAY);
		}
		
		_rsoGameState.value = GameState.EDIT;
    }

	private void OnEnable()
	{
		_rsePlayNextWave.action += PlayNextWave;
	}

	private void OnDisable()
	{
		_rsePlayNextWave.action -= PlayNextWave;
	}
}