using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Tweakable values")] 
    [SerializeField] private float _thresholdDistance;

    [Header("References")]
    [SerializeField] private RSO_Path _rsoPath;

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
            print("completed");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Hearth>(out var health))
        {
            health.UpdateHealth(-_enemyConfig.damage);
            Destroy(this.gameObject);
        }
    }
}