using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private RSO_Heart _rsoHeart;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private TilePlacer _tilePlacer;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private Road[] _roads;
    [SerializeField] private Tower[] _towers;

    private void Start()
    {
		_rsoHeart.value = _gameConfig.baseHeartAmount;
		_rsoPath.value = new List<Vector3>();

		_tilePlacer.Initialize();
		_enemyPool.Initialize();
		_waveManager.Initialize();
        
        for (int index = 0; index < _roads.Length; index++)
        {
            _roads[index].PlacePath();
        }

		for (int i = 0; i < _towers.Length; i++)
		{
			_towers[i].Initialize();
		}
    }
}