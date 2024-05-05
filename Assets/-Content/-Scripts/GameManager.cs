using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("RS references")]
    [SerializeField] private RSO_Path _rsoPath;
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_RoadPlaced _rsoRoadPlaced;
	[SerializeField] private RSO_TowerPlaced _rsoTowerPlaced;
	[SerializeField] private RSO_FlagPlaced _rsoFlagPlaced;
	[SerializeField] private RSO_EnemyKilled _rsoEnemyKilled;
	[SerializeField] private RSE_Sound _rseSoundPlay;

	[Header("Audio references")]
    [SerializeField] private AudioClip _music;

	[Header("Configs references")]
    [SerializeField] private GameConfig _gameConfig;
	
	[Header("Manager references")]
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private TilePlacer _tilePlacer;
	
	[Header("Gameplay blocks references")]
    [SerializeField] private Road[] _roads;

    private void Start()
    {
		_rsoPath.value = new List<Vector3>();
		
		_rsoRoadPlaced.value = 0;
		_rsoTowerPlaced.value = 0;
		_rsoFlagPlaced.value = 0;
		_rsoEnemyKilled.value = 0;

		_tilePlacer.Initialize();
		_enemyPool.Initialize();
		_waveManager.Initialize();

        _rseSoundPlay.Call(TypeSound.MUSIC, _music, true);
        
        for (int index = 0; index < _roads.Length; index++)
        {
            _roads[index].PlacePath();
        }
		
		_rsoGameState.value = GameState.EDIT;
    }
}