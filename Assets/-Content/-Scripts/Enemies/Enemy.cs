using System.Collections;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Tweakable values")] 
    [SerializeField] private float _thresholdDistance;
    [SerializeField] private AudioClip _onHitClip;
	[SerializeField] private float _deathScaleDuration = 0.25f;
	[SerializeField] private float _backProgressBarDuration = 0.5f;

    [Header("Internal references")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthBarLerp;

    [Header("External references")]
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private RSE_EnemyDies _rseEnemyDies;
    [SerializeField] private RSE_Sound _rsePlaySound;
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
		_healthBar.fillAmount = 1;
		_healthBarLerp.fillAmount = _healthBar.fillAmount;
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
			StartCoroutine(AnimateDeath());
		}
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

    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;

		if (_currentHealth <= 0)
		{
			HandleDeath();
		}

		if (EditorApplication.isPlaying) 
		{
			_healthBar.fillAmount = (float) _currentHealth / (float) _enemyConfig.health;
			_healthBarLerp.DOFillAmount(_healthBar.fillAmount, _backProgressBarDuration).SetEase(Ease.InOutSine);

			_rsePlaySound.Call(TypeSound.SFX, _onHitClip, false);

			_animator.SetTrigger("GetHit");
		}
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
		public int test_setCurrentHealth(int value) => _currentHealth = value;

	#endif
}