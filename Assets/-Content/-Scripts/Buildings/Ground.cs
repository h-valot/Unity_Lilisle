using UnityEngine;

public enum GroundType 
{
    DIRT,
    CITY,
    WATER
}

public class Ground : MonoBehaviour 
{
    public GroundType type;

    private void Start() 
    {
        RaycastHit[] hits = Physics.BoxCastAll(
			gameObject.transform.position, 
			new Vector3(0.25f, 0.25f, 0.25f), 
			new Vector3(0, -1, 0)
		);

		for (int i = 0; i < hits.Length; i++)
        {
			if (hits[i].collider.TryGetComponent<Ground>(out Ground ground))
            {
                if (ground != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}