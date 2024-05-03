﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("RSO references")]
    [SerializeField] private RSO_Heart _rsoHeart;
    [SerializeField] private RSO_Path _rsoPath;
	[SerializeField] private RSO_HandCard _rsoHandCard;
	[SerializeField] private RSO_GameState _rsoGameState;
	
	[Header("Configs references")]
    [SerializeField] private GameConfig _gameConfig;
	
	[Header("Manager references")]
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private TilePlacer _tilePlacer;
	
	[Header("Gameplay blocks references")]
    [SerializeField] private Road[] _roads;
    [SerializeField] private Tower[] _towers;

    private void Start()
    {
		_rsoHeart.value = _gameConfig.baseHeartAmount;
		_rsoPath.value = new List<Vector3>();
		_rsoHandCard.value = new List<TileConfig>();
		_rsoGameState.value = GameState.EDIT;

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

	private void ClampHeart()
	{
		if (_rsoHeart.value > _gameConfig.baseHeartAmount)
		{
			_rsoHeart.value = _gameConfig.baseHeartAmount;
		}

		if (_rsoHeart.value < 0)
		{
			_rsoHeart.value = 0;
		}
	}

	private void OnEnable()
	{
		_rsoHeart.OnChanged += ClampHeart;
	}

	private void OnDisable()
	{
		_rsoHeart.OnChanged -= ClampHeart;
	}
}