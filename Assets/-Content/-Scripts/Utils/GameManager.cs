﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private TilePlacer _tilePlacer;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Road[] _roads;
    [SerializeField] private Tower[] _towers;

    private void Start()
    {
		_tilePlacer.Initialize();
		_enemyPool.Initialize();
        _rsoPath.value = new List<Vector3>();
        
        for (int index = 0; index < _roads.Length; index++)
        {
            _roads[index].PlacePath();
        }

		for (int i = 0; i < _towers.Length; i++)
		{
			_towers[i].Initialize();
		}
        
        _enemyGenerator.SpawnWave();
    }
}