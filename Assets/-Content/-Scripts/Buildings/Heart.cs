using UnityEngine;

public class Heart : Road
{ 
	[Header("HEART")]

    [Header("Tweakable values")]
    [SerializeField] private AudioClip _onHitClip;
    [SerializeField] private AudioClip _onDeathClip;

    [Header("External references")]
    [SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_GameState _rsoGameState;
    [SerializeField] private RSE_Sound _rsePlaySound;
    [SerializeField] private GameConfig _gameConfig;

	private void Start()
	{
		_rsoHeart.value = _gameConfig.baseHeartAmount;
	}

    public void UpdateHeart(int amount)
    {
        _rsoHeart.value += amount;
		_rsePlaySound.Call(TypeSound.SFX, _onHitClip, false);
    }

	public void HandleHeart()
	{
		if (_rsoHeart.value <= 0)
		{
			_rsoGameState.value = GameState.GAME_OVER;
			_rsePlaySound.Call(TypeSound.SFX, _onDeathClip, false);
			return;
		}
	}

	private void OnEnable()
	{
		_rsoHeart.OnChanged += HandleHeart;
	}

	private void OnDisable()
	{
		_rsoHeart.OnChanged -= HandleHeart;
	}

	#if UNITY_EDITOR

		public void test_setRsoHeart(RSO_Heart value) => _rsoHeart = value;
		public void test_setRsoGameState(RSO_GameState value) => _rsoGameState = value;

	#endif
}