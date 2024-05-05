using UnityEngine;

public class BillboardUI : MonoBehaviour
{
	private Transform _camera;

	private void Start()
	{
		_camera = Camera.main.transform;
	}

	private void Update()
	{
		transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
	}
}