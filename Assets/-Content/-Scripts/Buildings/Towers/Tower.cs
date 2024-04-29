using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private TowerConfig _towerConfig;

	[Header("References")]
	[SerializeField] private Transform _shootingPoint; 
	[SerializeField] private SphereCollider _sphereCollider;

	[Header("Debugging")]
	public List<Enemy> _enemiesInRange = new List<Enemy>();

	private Enemy _target;
	private bool _reloading;

	public void Initialize()
	{
		_sphereCollider.radius = _towerConfig.atkRange;
		_sphereCollider.center = _shootingPoint.position;
	}

	private void Update()
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

		_target.healthComponent.UpdateHealth(-_towerConfig.damage, CheckEnemyHealth);
		StartCoroutine(Reload());
	}
	
	private IEnumerator Reload()
	{
		_reloading = true;
		yield return new WaitForSeconds(_towerConfig.fireRate);
		_reloading = false;
	}

	private void CheckEnemyHealth(int health)
	{
		if (health > 0) 
		{
			return;
		}
		
		_enemiesInRange.Remove(_target);
		_target = null;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.TryGetComponent<Enemy>(out var enemy))
		{
			_enemiesInRange.Add(enemy);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.TryGetComponent<Enemy>(out var enemy))
		{
			_enemiesInRange.Remove(enemy);
		}
	}

	private void OnDrawGizmos()
	{
        Gizmos.DrawWireSphere(transform.position, _towerConfig.atkRange);
	}
}