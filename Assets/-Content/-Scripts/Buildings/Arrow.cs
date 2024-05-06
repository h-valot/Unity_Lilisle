using UnityEngine;

public class Arrow : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private float _moveSpeed;

	private Transform _target;
	private int _damage;
	private bool _isLaunched;
	private Vector3 _movementDirection;

	public void Launch(Transform target, int damage)
	{
		_target = target;
		_damage = damage;
		_isLaunched = true;
	}

	private void FixedUpdate()
	{
		if (!_isLaunched)
		{
			return;
		}

		HandleTargetDeath();
		Move();
	}

	private void HandleTargetDeath()
	{
		if (_target.position == Vector3.zero)
		{
			Destroy(gameObject);
		}
	}

	private void Move()
	{
        _movementDirection = _target.position - transform.position;
        _movementDirection = _movementDirection.normalized * (_moveSpeed * Time.fixedDeltaTime);
        transform.position += _movementDirection;
        
        transform.LookAt(_target.position);
	}

	private void OnTriggerEnter(Collider collider)
	{
        if (collider.TryGetComponent<Enemy>(out var enemy))
        {
			enemy.UpdateHealth(-_damage);
			Destroy(gameObject);
        }
	}
}
