using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private int _poolCapacity = 20;

	[Header("References")]
	[SerializeField] private GameObject _enemyPrefab;

	private List<Enemy> _pooledObjects;

	public void Initialize()
	{
		_pooledObjects = new List<Enemy>();

		for (int i = 0; i < _poolCapacity; i++)
		{
			Enemy newEnemy = Instantiate(_enemyPrefab, transform).GetComponent<Enemy>();
			newEnemy.gameObject.SetActive(false);
			_pooledObjects.Add(newEnemy);
		}
	}

	public bool IsEnemiesAlived()
	{
		int enemyCount = 0;
		foreach (var enemy in _pooledObjects)
		{
			if (enemy.gameObject.activeInHierarchy)
			{
				enemyCount++;
			}
		}
		return enemyCount > 0;
	}

	public Enemy Get()
	{
		Enemy pooledObject = null;
		
		for (int i = 0; i < _poolCapacity; i++)
		{
			if (!_pooledObjects[i].gameObject.activeInHierarchy)
			{
				pooledObject = _pooledObjects[i];
			}
		}
		
		if (pooledObject == null)
		{
			pooledObject = Instantiate(_enemyPrefab).GetComponent<Enemy>();
			_pooledObjects.Add(pooledObject);
		}
		
		pooledObject.gameObject.SetActive(true);
		return pooledObject;
	}
}