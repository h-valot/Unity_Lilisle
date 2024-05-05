using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private float _lerpScalar;

	[Header("External references")]
	[SerializeField] private Transform _mentor;

	private void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, _mentor.position, _lerpScalar);
	}
}