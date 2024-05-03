using UnityEngine;

public class Pin : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform _raycastPoint;

	private RaycastHit[] _hits;
	private Ray _ray;
	private bool _connectorHits;

	public bool IsLinked()
	{
		_ray = new Ray(_raycastPoint.position, (transform.position - _raycastPoint.position).normalized);
		_hits = new RaycastHit[5];
		Physics.RaycastNonAlloc(_ray, _hits);

		if (_hits.Length <= 0)
		{
			return false;
		}
		
		_connectorHits = false;
		for (int i = 0; i < _hits.Length; i++)
		{
			if (_hits[i].collider == null)
			{
				continue;
			}

			if (!_hits[i].collider.TryGetComponent<Connector>(out var externalConnector))
			{
				continue;
			}

			if (externalConnector == transform.parent.GetComponent<Connector>())
			{
				continue;
			}

			_connectorHits = true;
		}

		if (!_connectorHits)
		{
			return false;
		}

		return true;
	}
}