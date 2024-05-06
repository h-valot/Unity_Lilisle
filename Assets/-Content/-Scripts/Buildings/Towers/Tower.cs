using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tile3D
{
	[Header("TOWER")]

	[Header("Tweakable values")]
	[SerializeField] private TowerConfig _towerConfig;

	[Header("Internal references")]
	[SerializeField] private Transform _shootingPoint; 
	[SerializeField] private SphereCollider _sphereCollider;

	[Header("External references")]
	[SerializeField] private RSE_EnemyDies _rseEnemyDies;
	[SerializeField] private RSO_TowerPlaced _rsoTowerPlaced;
	[SerializeField] private Arrow _arrowPrefab;

	private List<Enemy> _enemiesInRange = new List<Enemy>();
	private Enemy _target;
	private bool _reloading;

	protected override void TileStart()
	{
		_sphereCollider.radius = _towerConfig.atkRange;
		_sphereCollider.center = _shootingPoint.localPosition;
		_reloading = false;
	}

	protected override void TileUpdate()
	{
		if (_reloading)
		{
			return;
		}

		GetTarget();
		Attack();
	}

	private void GetTarget()
	{
		_target = null;
		float shortestDistanceFromHearth = 9999;

		for (int i = 0; i < _enemiesInRange.Count; i++)
		{
			float distanceFromHearth = _enemiesInRange[i].GetDistanceFromLastWaypoint();
			if (shortestDistanceFromHearth > distanceFromHearth)
			{
				shortestDistanceFromHearth = distanceFromHearth;
				_target = _enemiesInRange[i];
			}
		}
	}

	private void Attack()
	{
		if (_target == null)
		{
			return;
		}

		Arrow newArrow = Instantiate(_arrowPrefab, _shootingPoint.transform);
		newArrow.Launch(_target._arrowTarget, _towerConfig.damage);
		StartCoroutine(Reload());
	}
	
	private IEnumerator Reload()
	{
		_reloading = true;
		yield return new WaitForSeconds(_towerConfig.fireRate);
		_reloading = false;
	}

	private void RemoveEnemy(Enemy enemy)
	{
		_enemiesInRange.Remove(enemy);
	}

    public override void DoPlacementAction(Tile3D[] surroundTiles, Ground belowGround) 
    {
		_rsoTowerPlaced.value++;
    }

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.TryGetComponent<Enemy>(out var enemy))
		{
			if (_enemiesInRange.Contains(enemy))
			{
				return;
			}

			_enemiesInRange.Add(enemy);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.TryGetComponent<Enemy>(out var enemy))
		{
			RemoveEnemy(enemy);
		}
	}

	private void OnEnable()
	{
		_rseEnemyDies.action += RemoveEnemy;
	}

	private void OnDisable()
	{
		_rseEnemyDies.action -= RemoveEnemy;
	}
}