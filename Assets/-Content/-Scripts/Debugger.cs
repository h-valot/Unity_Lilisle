using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private RSO_Path _rsoPath;
    [SerializeField] private Path[] _paths;
    [SerializeField] private Enemy[] _enemies;

    private void Start()
    {
        _rsoPath.value = new List<Vector3>();
        
        for (int index = 0; index < _paths.Length; index++)
        {
            _paths[index].Place();
        }
        
        for (int index = 0; index < _enemies.Length; index++)
        {
            _enemies[index].Initialize();
        }
    }
}