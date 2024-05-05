using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private TextMeshProUGUI _tmpScore;
	[SerializeField] private TextMeshProUGUI _tmpTilePlaced;
	[SerializeField] private TextMeshProUGUI _tmpRoadPlaced;
	[SerializeField] private TextMeshProUGUI _tmpTowerPlaced;
	[SerializeField] private TextMeshProUGUI _tmpFlagPlaced;
	[SerializeField] private TextMeshProUGUI _tmpEnemyKilled;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private RSO_RoadPlaced _rsoRoadPlaced;
	[SerializeField] private RSO_TowerPlaced _rsoTowerPlaced;
	[SerializeField] private RSO_FlagPlaced _rsoFlagPlaced;
	[SerializeField] private RSO_EnemyKilled _rsoEnemyKilled;

	private void HandleGameOver()
	{
		if (_rsoGameState.value == GameState.GAME_OVER)
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
		Show();

		_tmpScore.text = $"{_rsoCurrentWave.value}";
		_tmpTilePlaced.text = $"{_rsoRoadPlaced.value + _rsoTowerPlaced.value + _rsoFlagPlaced.value}";
		_tmpRoadPlaced.text = $"{_rsoRoadPlaced.value}";
		_tmpTowerPlaced.text = $"{_rsoTowerPlaced.value}";
		_tmpFlagPlaced.text = $"{_rsoFlagPlaced.value}";
		_tmpEnemyKilled.text = $"{_rsoEnemyKilled.value}";
	}

	public void Retry()
	{
		SceneManager.LoadScene("Game");
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleGameOver;
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleGameOver;
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