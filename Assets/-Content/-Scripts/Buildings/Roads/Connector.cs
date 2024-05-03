using UnityEngine;

public class Connector : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform _raycastPoint;
	[SerializeField] private Pin[] _internalPins;
	[SerializeField] private BoxCollider _boxCollider;

	private RaycastHit[] _hits;
	private Ray _ray;
	private	bool _externalPinHits;
	private bool _internalPinHits;
	private int _internalPinLinked;

	public bool IsLinked() => IsConnectorLinked() && IsPinLinked();

	private bool IsConnectorLinked()
	{
		_ray = new Ray(_raycastPoint.position, (transform.position - _raycastPoint.position).normalized);
		_hits = new RaycastHit[5];
		Physics.RaycastNonAlloc(_ray, _hits);

		if (_hits.Length <= 0)
		{
			return false;
		}
		
		_externalPinHits = false;
		for (int i = 0; i < _hits.Length; i++)
		{
			if (_hits[i].collider == null)
			{
				continue;
			}

			if (!_hits[i].collider.TryGetComponent<Pin>(out var externalPin))
			{
				continue;
			}

			_internalPinHits = false;
			for (int j = 0; j < _internalPins.Length; j++)
			{
				if (externalPin == _internalPins[j])
				{
					_internalPinHits = true;
					continue;
				}
			}

			if (_internalPinHits)
			{
				continue;
			}

			_externalPinHits = true;
		}

		if (!_externalPinHits)
		{
			return false;
		}

		return true;
	}

	private bool IsPinLinked()
	{
		_internalPinLinked = 0;
		for (int i = 0; i < _internalPins.Length; i++)
		{
			if (_internalPins[i].IsLinked())
			{
				_internalPinLinked++;
			}
		}
		
		if (_internalPinLinked <= 0)
		{
			return false;
		}

		return true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position + _boxCollider.center, _boxCollider.size);
		Gizmos.DrawRay(_ray);
	}
}