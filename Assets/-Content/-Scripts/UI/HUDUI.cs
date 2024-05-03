using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private GameObject _goWave;
	[SerializeField] private TextMeshProUGUI _tmpWaveCounter;
	[SerializeField] private TextMeshProUGUI _tmpHeartCounter;
	[SerializeField] private Image _imgHeartProgressBar;

	[Header("External references")]
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private RSE_HideHUD _rseHideHUD;
	[SerializeField] private GameConfig _gameConfig;

	private void UpdateHeart()
	{
		_tmpHeartCounter.text = $"{_rsoHeart.value}";
		_imgHeartProgressBar.fillAmount = (float)_rsoHeart.value / (float)_gameConfig.baseHeartAmount;
	}

	private void UpdateWave()
	{
		_goWave.SetActive(_rsoCurrentWave.value >= 0);
		_tmpWaveCounter.text = $"{_rsoCurrentWave.value + 1}";
	}

	private void HideHUD(bool hidden)
	{
		if (hidden)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	private void OnEnable()
	{
		_rsoHeart.OnChanged += UpdateHeart;
		_rsoCurrentWave.OnChanged += UpdateWave;
		_rseHideHUD.action += HideHUD;
	}

	private void OnDisable()
	{
		_rsoHeart.OnChanged -= UpdateHeart;
		_rsoCurrentWave.OnChanged -= UpdateWave;
		_rseHideHUD.action -= HideHUD;
	}

	private void Show()
	{
		_goParent.SetActive(true);
	}

	private void Hide()
	{
		_goParent.SetActive(false);
	}
}
