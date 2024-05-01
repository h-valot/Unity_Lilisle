using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private TextMeshProUGUI _tmpWaveCounter;
	[SerializeField] private GameObject _goWave;
	[SerializeField] private TextMeshProUGUI _tmpHeartCounter;
	[SerializeField] private Image _imgHeartProgressBar;

	[Header("External references")]
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private GameConfig _gameConfig;

	private void OnEnable()
	{
		_rsoHeart.OnChanged += UpdateHeart;
		_rsoCurrentWave.OnChanged += UpdateWave;
	}

	private void OnDisable()
	{
		_rsoHeart.OnChanged -= UpdateHeart;
		_rsoCurrentWave.OnChanged -= UpdateWave;
	}

	private void UpdateDisplays()
	{
		UpdateHeart();
		UpdateWave();
	}

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
}
