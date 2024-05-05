using UnityEngine;

public class Heart : MonoBehaviour
{ 
    [Header("Tweakable values")]
    [SerializeField] private AudioClip _onHitClip;
    [SerializeField] private AudioClip _onDeathClip;

    [Header("External references")]
    [SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_GameState _rsoGameState;
    [SerializeField] private RSE_Sound _rsePlaySound;
    [SerializeField] private GameConfig _gameConfig;

	private int newHeartAmount;

	private void Start()
	{
		_rsoHeart.value = _gameConfig.baseHeartAmount;
	}

    public void UpdateHeart(int amount)
    {
		newHeartAmount = _rsoHeart.value + amount;

		if (newHeartAmount >= _gameConfig.baseHeartAmount)
		{
			newHeartAmount = _gameConfig.baseHeartAmount;
		}
		
		if (newHeartAmount <= 0)
		{
			newHeartAmount = 0;
			_rsoHeart.value = newHeartAmount;
			_rsoGameState.value = GameState.GAME_OVER;
			_rsePlaySound.Call(TypeSound.SFX, _onDeathClip, false);
			return;
		}

        _rsoHeart.value = newHeartAmount;
		_rsePlaySound.Call(TypeSound.SFX, _onHitClip, false);
    }
}