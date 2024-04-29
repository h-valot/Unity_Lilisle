using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private int _poolCapacity = 20;

	[Header("References")]
	[SerializeField] private GameObject _enemyPrefab;

	private List<Enemy> _pooledObjects;

	private void Start()
	{
		_pooledObjects = new List<Enemy>();

		for (int i = 0; i < _poolCapacity; i++)
		{
			Enemy newEnemy = Instantiate(_enemyPrefab).GetComponent<Enemy>();
			newEnemy.gameObject.SetActive(false);
			_pooledObjects.Add(newEnemy);
		}
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