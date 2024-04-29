using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveConfig[] _waves;
	[SerializeField] private EnemyPool _enemyPool;

    private int _currentWave;

    public void SpawnWave()
    {
        StartCoroutine(SpawnWaveWithDelay());
    }

    private IEnumerator SpawnWaveWithDelay()
    {
        for (int i = 0; i < _waves[_currentWave].enemies.Length; i++)
        {
            Enemy newEnemy = _enemyPool.Get();
            newEnemy.Initialize(_waves[_currentWave].enemies[i]);
            yield return new WaitForSeconds(_waves[_currentWave].enemies[i].delay);
        }
    }
}