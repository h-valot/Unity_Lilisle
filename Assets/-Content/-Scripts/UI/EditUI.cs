using UnityEngine;

public class EditUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private CardUI[] _cardsUI;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_HandCard _rsoHandCard;


	private void HandleEdit()
	{
		if (_rsoGameState.value == GameState.EDIT)
		{
			Show();
		}
		else 
		{
			Hide();
		}
	}

	private void AddReward(TileConfig newTileConfig)
	{
		// Do nothing
	}

	private void RemoveReward(TileConfig newTileConfig)
	{
		// Do nothing
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleEdit;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect += AddReward;
			_cardsUI[i].OnUnselect += RemoveReward;
		}
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleEdit;

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

	public void Show()
	{
		_goParent.SetActive(true);
	}
}
