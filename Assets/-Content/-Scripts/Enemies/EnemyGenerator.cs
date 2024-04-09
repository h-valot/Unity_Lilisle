using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private WaveConfig[] _waves;

    private int _currentWave;

    public void SpawnWave()
    {
        StartCoroutine(SpawnWaveWithDelay());
    }

    private IEnumerator SpawnWaveWithDelay()
    {
        for (int index = 0; index < _waves[_currentWave].enemies.Length; index++)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab);
            newEnemy.Initialize(_waves[_currentWave].enemies[index]);
            yield return new WaitForSeconds(_waves[_currentWave].enemies[index].delay);
        }
    }
}