using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private TextMeshProUGUI _tmpScore, _tmpTitle;

	[Header("External references")]
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;

	private int _heartStart;
	private int _heartDelta;

	private void HandleHeartModification()
	{
		if (_rsoCurrentWave.value < 0)
		{
			Hide();
			return;
		}

		if (_rsoGameState.value == GameState.WAVE)
		{
			_heartStart = _rsoHeart.value;
			Hide();
			return;
		}

		if (_rsoGameState.value == GameState.EDIT)
		{
			_heartDelta = _rsoHeart.value - _heartStart;
			Show();
			RefreshDisplays();
			return;
		}
	}

	private void RefreshDisplays()
	{
		if (_heartDelta >= 0)
		{
			_tmpTitle.text = $"Defense won";
			_tmpScore.text = $"You win 1";
			_rsoHeart.value++;
		}
		else
		{
			Hide();
		}
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleHeartModification;
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleHeartModification;
	}

	private void Hide()
	{
		_goParent.SetActive(false);
	}

	private void Show()
	{
		_goParent.SetActive(true);
	}
}