using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
	[Header("Tweakable values")]
	[SerializeField] private float _gainHeartDuration;

	[Header("Internal references")]
	[SerializeField] private GameObject _goGainHeart;
	[SerializeField] private GameObject _goReward;
	[SerializeField] private TextMeshProUGUI _tmpConfirm;
	[SerializeField] private CardUI[] _cardsUI;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_HandCard _rsoHandCard;
	[SerializeField] private RewardsConfig _rewardsConfig;

	private int _lastHeartAmount;

	private void HandleReward()
	{
		if (_rsoGameState.value == GameState.REWARD)
		{
			StartCoroutine(Show());
		}
		else 
		{
			Hide();
		}
	}

	private IEnumerator Show()
	{
		if (_lastHeartAmount == _rsoHeart.value)
		{
			_rsoHeart.value++;
			_goGainHeart.SetActive(true);
			yield return new WaitForSeconds(_gainHeartDuration);
			_goGainHeart.SetActive(false);
		}
		_lastHeartAmount = _rsoHeart.value;

		_goReward.SetActive(true);
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].Initialize(_rewardsConfig.tiles[Random.Range(0, _rewardsConfig.tiles.Length)]);
		}
	}

	public void ConfirmReward()
	{
		int totalCost = 0;
		for (int i = 0; i < _rsoHandCard.value.Count; i++)
		{
			totalCost += _rsoHandCard.value[i].cost;
		} 
		_rsoHeart.value -= totalCost;

		_rsoGameState.value = GameState.EDIT;
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleReward;
		_lastHeartAmount = _rsoHeart.value;
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleReward;
	}

	public void Hide()
	{
		_goGainHeart.SetActive(false);
		_goReward.SetActive(false);
	}
}
