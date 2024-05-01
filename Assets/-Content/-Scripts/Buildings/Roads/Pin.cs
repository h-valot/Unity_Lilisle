using UnityEngine;

public enum PinSignature
{
	IN = 0,
	OUT
}

public class Pin : MonoBehaviour
{
	[Header("Tweakable value")]
	public PinSignature signature;

	[Header("References")]
	[SerializeField] private Transform _raycastPoint;

	[Header("Debugging")]
	public bool connected;

	public void VerifyConnection()
	{
		connected = false;

		if (!Physics.Raycast(_raycastPoint.position, (transform.position - _raycastPoint.position).normalized, out var hit))
		{
			return;
		}

		if (!hit.collider.TryGetComponent<Pin>(out var otherPin))
		{
			return;
		}

		if (signature == PinSignature.OUT
		&& otherPin.signature == PinSignature.IN)
		{
			connected = true;
		}
	}
}