using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [Header("Tweakable values")] 
    [SerializeField] private float _thresholdDistance;

	[Header("Animations")]
	[SerializeField] private float _deathScaleDuration = 0.25f;

    [Header("Intenal references")]
    [SerializeField] private Animator _animator;
	public Health healthComponent;

    [Header("External references")]
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private RSE_EnemyDies _rseEnemyDies;

    private EnemyConfig _enemyConfig;
    private Vector3 _movementDirection;
    private bool _initialized;
    private bool _completed;
    
	[Header("Debugging")]
    public int _nextWaypoint;

    public void Initialize(EnemyConfig newEnemyConfig)
    {
        _enemyConfig = newEnemyConfig;
        transform.position = _rsoPath.value[^1];
        _nextWaypoint = _rsoPath.value.Count - 2;
		_completed = false;
        _initialized = true;
    }

    private void FixedUpdate()
    {
        if (!_initialized
        || _completed)
        {
            return;
        }
        
        Move();
        CheckForDistance();
    }

    private void Move()
    {
        _movementDirection = _rsoPath.value[_nextWaypoint] - transform.position;
        _movementDirection = _movementDirection.normalized * (_enemyConfig.speed * Time.fixedDeltaTime);
        transform.position += _movementDirection;
        
        transform.LookAt(_rsoPath.value[_nextWaypoint]);
    }

    private void CheckForDistance()
    {
        if ((transform.position - _rsoPath.value[_nextWaypoint]).magnitude > _thresholdDistance)
        {
            return;
        }

        _nextWaypoint--;

        if (_nextWaypoint < 0)
        {
            _completed = true;
        }
    }

	public float GetDistanceFromLastWaypoint()
	{
		float totalDistance = 0f;
		for (int i = 0; i < _rsoPath.value.Count; i++)
		{
			if (_nextWaypoint == i)
			{
				totalDistance += Vector3.Distance(transform.position, _rsoPath.value[i]);
				break;
			}

			if (i + 1 >= _rsoPath.value.Count)
			{
				break;
			}

			totalDistance += Vector3.Distance(_rsoPath.value[i + 1], _rsoPath.value[i]);
		}
		return totalDistance;
	}

	public void HandleDeath()
	{
		_rseEnemyDies.Call(this);
		StartCoroutine(AnimateDeath());
	}


	public IEnumerator AnimateDeath()
	{
		transform.DOScale(0, _deathScaleDuration).SetEase(Ease.OutElastic);
		yield return new WaitForSeconds(_deathScaleDuration);

		transform.localScale = Vector3.one;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		gameObject.SetActive(false);
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Heart>(out var health))
        {
            health.UpdateHealth(-_enemyConfig.damage);
			HandleDeath();
        }
    }
}