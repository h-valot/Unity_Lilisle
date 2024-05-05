using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SettingsUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goMenuParent;

	public void LoadSceneMenu()
	{
		DOTween.CompleteAll();
		DOTween.KillAll();
		SceneManager.LoadScene("Menu");
	}

	public void HideMenu()
	{
		_goMenuParent.SetActive(false);
	}

	public void ShowMenu()
	{
		_goMenuParent.SetActive(true);
	}
}