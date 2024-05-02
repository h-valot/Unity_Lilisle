using UnityEngine;

public class EditUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;

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
		_rsoGameState.OnChanged += HandleEdit;
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleEdit;
	}
}
