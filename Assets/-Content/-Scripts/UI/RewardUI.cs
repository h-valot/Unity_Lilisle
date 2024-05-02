using UnityEngine;

public class RewardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;

	private void HandleReward()
	{
		if (_rsoGameState.value == GameState.REWARD)
		{
			Show();
		}
		else 
		{
			Hide();
		}
	}

	public void ConfirmReward()
	{
		_rsoGameState.value = GameState.EDIT;
	}

	public void Hide()
	{
		_goParent.SetActive(false);
	}

	public void Show()
	{
		_goParent.SetActive(true);
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleReward;
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleReward;
	}
}
