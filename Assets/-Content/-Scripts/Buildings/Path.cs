using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;

    public void Place()
    {
        _rsoPath.value.Add(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}