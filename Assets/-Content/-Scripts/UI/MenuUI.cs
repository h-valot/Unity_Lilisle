using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuUI : MonoBehaviour
{

	public void Play()
	{
		DOTween.CompleteAll();
		DOTween.KillAll();
		SceneManager.LoadScene("Game");
	}

	public void Exit()
	{
		Application.Quit();
	}
}