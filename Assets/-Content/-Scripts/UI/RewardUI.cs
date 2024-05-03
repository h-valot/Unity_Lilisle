using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private TextMeshProUGUI _tmpConfirm;
	[SerializeField] private CardUI[] _cardsUI;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_HandCard _rsoHandCard;
	[SerializeField] private RewardsConfig _rewardsConfig;

	private void HandleReward()
	{
		if (_rsoGameState.value == GameState.REWARD)
		{
			Initialize();
		}
		else 
		{
			Hide();
		}
	}

	private void Initialize()
	{
		_goParent.SetActive(true);
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

	private void AddReward(TileConfig newTileConfig)
	{
		_rsoHandCard.value.Add(newTileConfig);
	}

	private void RemoveReward(TileConfig newTileConfig)
	{
		_rsoHandCard.value.Remove(newTileConfig);
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleReward;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect += AddReward;
			_cardsUI[i].OnUnselect += RemoveReward;
		}
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleReward;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect -= AddReward;
			_cardsUI[i].OnUnselect -= RemoveReward;
		}
	}

	public void Hide()
	{
		_goParent.SetActive(false);
	}
}
