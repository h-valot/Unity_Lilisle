using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;

    public void Place()
    {
        _rsoPath.value.Add(transform.position);
    }
}