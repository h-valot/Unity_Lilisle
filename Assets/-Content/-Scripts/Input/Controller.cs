using UnityEngine;

public class Controller : MonoBehaviour 
{
    [Header("Tweakable values")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("References")]
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _input;

    private void Update() 
    {
		// Gather inputs
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0,  Input.GetAxisRaw("Vertical"));
    }

    private void LateUpdate() 
    {
		// Move
        _rigidbody.MovePosition(_rigidbody.position + _input * moveSpeed * Time.fixedDeltaTime);
    }
}