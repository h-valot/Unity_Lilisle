using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private RSO_Path _rsoPath;

    [Header("Tweak-able values")] 
    [SerializeField] private float _thresholdDistance;

    public int _nextWaypoint;
    private Vector3 _movementDirection;
    private bool _initialized;
    private bool _completed;

    public void Initialize()
    {
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
}