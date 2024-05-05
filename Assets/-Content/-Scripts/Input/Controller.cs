using UnityEngine;

public class Controller : MonoBehaviour 
{
    [Header("Tweakable values")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("References")]
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _input;

    private void FixedUpdate() 
    {
		GatherInputs();
		Move();
    }

    private void GatherInputs() 
    {
        _input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    }

	private void Move()
	{
		Quaternion rotation = Quaternion.Euler(0, 45.0f, 0);
		Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
		Vector3 direction = isoMatrix.MultiplyPoint3x4(_input);
		_rigidbody.velocity = direction.normalized * moveSpeed;
	}
}