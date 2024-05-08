using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Tweakable values")] 
    [SerializeField] private float _thresholdDistance;
    [SerializeField] private AudioClip _onHitClip;
	[SerializeField] private float _backProgressBarDuration = 0.5f;

    [Header("Internal references")]
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Image _healthBarLerp;
    public Transform _arrowTarget;

    [Header("External references")]
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private RSE_EnemyDies _rseEnemyDies;
    [SerializeField] private RSE_PlaySound _rsePlaySound;
	[SerializeField] private RSO_EnemyKilled _rsoEnemyKilled;
 
    private int _nextWaypoint;
    private EnemyConfig _enemyConfig;
    private Vector3 _movementDirection;
    private bool _initialized;
    private bool _completed;
	private int _currentHealth;
    private Animator _animator;
	private GameObject _mesh;

    public void Initialize(EnemyConfig newEnemyConfig)
    {
        _enemyConfig = newEnemyConfig;
        transform.position = _rsoPath.value[^1];
        _nextWaypoint = _rsoPath.value.Count - 2;
		_currentHealth = _enemyConfig.health;
		_healthBarFill.fillAmount = 1;
		_healthBarLerp.fillAmount = _healthBarFill.fillAmount;
		_completed = false;

		if (_mesh) 
		{ 
			Destroy(_mesh); 
		}
		_mesh = Instantiate(_enemyConfig.mesh, transform);
		_animator = _mesh.GetComponent<Animator>();

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
		_rsoEnemyKilled.value++;
		if (gameObject.activeInHierarchy)
		{
			transform.localScale = Vector3.one;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			gameObject.SetActive(false);
		}
	}

    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;

		if (_currentHealth <= 0)
		{
			HandleDeath();
		}

		_healthBarFill.fillAmount = (float) _currentHealth / (float) _enemyConfig.health;
		_healthBarLerp.DOFillAmount(_healthBarFill.fillAmount, _backProgressBarDuration).SetEase(Ease.InOutSine);

		_rsePlaySound.Call(new Sound(_onHitClip, AudioChannel.SFX));

		_animator.SetTrigger("GetHit");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Heart>(out var health))
        {
            health.UpdateHeart(-_enemyConfig.damage);
			HandleDeath();
        }
    }

	#if UNITY_EDITOR

		public int test_getCurrentHealth() => _currentHealth;
		public int test_getNextWaypoint() => _nextWaypoint;
		public bool test_getCompleted() => _completed;
		public RSO_Path test_getRsoPath() => _rsoPath;

		public void test_setCurrentHealth(int value) => _currentHealth = value;
		public void test_setAnimator(Animator value) => _animator = value;
		public void test_setEnemyConfig(EnemyConfig value) => _enemyConfig = value;
		public void test_setMesh(GameObject value) => _mesh = value;
		public void test_setRsoEnemyKilled(RSO_EnemyKilled value) => _rsoEnemyKilled = value;
		public void test_setNextWaypoint(int value) => _nextWaypoint = value;
		public void test_setRsoPath(RSO_Path value) => _rsoPath = value;

		public void test_checkForDistance() => CheckForDistance();
		
	#endif
}