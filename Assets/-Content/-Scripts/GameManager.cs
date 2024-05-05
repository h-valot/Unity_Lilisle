using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("RSO references")]
    [SerializeField] private RSO_Heart _rsoHeart;
    [SerializeField] private RSO_Path _rsoPath;
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSE_Sound _rseSoundPlay;

	[Header("Audio references")]
    [SerializeField] private AudioClip _music;

	[Header("Configs references")]
    [SerializeField] private GameConfig _gameConfig;
	
	[Header("Manager references")]
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private TilePlacer _tilePlacer;
	
	[Header("Gameplay blocks references")]
    [SerializeField] private Road[] _roads;

    private void Start()
    {
		_rsoPath.value = new List<Vector3>();

		_tilePlacer.Initialize();
		_enemyPool.Initialize();
		_waveManager.Initialize();

        _rseSoundPlay.Call(TypeSound.MUSIC, _music, true);
        
        for (int index = 0; index < _roads.Length; index++)
        {
            _roads[index].PlacePath();
        }
		
		_rsoGameState.value = GameState.EDIT;
    }

	private void HandleGameOver()
	{
		if (_rsoHeart.value <= 0)
		{
			_rsoGameState.value = GameState.GAME_OVER;
		}
	}

	private void OnEnable()
	{
		_rsoHeart.OnChanged += HandleGameOver;
	}

	private void OnDisable()
	{
		_rsoHeart.OnChanged -= HandleGameOver;
	}
}