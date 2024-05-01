using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;

    public void Place()
    {
        _rsoPath.value.Add(transform.position);
    }
}