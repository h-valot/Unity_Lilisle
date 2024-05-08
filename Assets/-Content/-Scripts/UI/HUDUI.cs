using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
	[Header("Tweakables values")]
	[SerializeField] private float _backProgressBarDuration = 0.5f;

	[Header("Internal references")]
	[SerializeField] private GameObject _goGraphics;
	[SerializeField] private GameObject _goWave;
	[SerializeField] private TextMeshProUGUI _tmpWaveCounter;
	[SerializeField] private TextMeshProUGUI _tmpHeartCounter;
	[SerializeField] private Image _imgHeartProgressBar;
	[SerializeField] private Image _imgHeartProgressBarBack;

	[Header("External references")]
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private GameConfig _gameConfig;

	private void UpdateHeart()
	{
		_tmpHeartCounter.text = $"{Mathf.Clamp(_rsoHeart.value, 0, _gameConfig.baseHeartAmount)}";
		_imgHeartProgressBar.fillAmount = (float)_rsoHeart.value / (float)_gameConfig.baseHeartAmount;
		_imgHeartProgressBarBack.DOFillAmount(_imgHeartProgressBar.fillAmount, _backProgressBarDuration).SetEase(Ease.InOutSine);
	}

	private void UpdateWave()
	{
		_goWave.SetActive(_rsoCurrentWave.value >= 0);
		_tmpWaveCounter.text = $"{_rsoCurrentWave.value + 1}";
	}

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
}