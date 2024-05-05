using UnityEngine;

public class SettingsUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goMenuParent;

	public void HideMenu()
	{
		_goMenuParent.SetActive(false);
	}

	public void ShowMenu()
	{
		_goMenuParent.SetActive(true);
	}
}