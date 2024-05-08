using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goGraphics;
	[SerializeField] private GameObject _goLock;
	[SerializeField] private Image _imgBorder;
	[SerializeField] private Image _imgIcon;
	[SerializeField] private TextMeshProUGUI _tmpTitle;
	[SerializeField] private TextMeshProUGUI _tmpFlavor;
	[SerializeField] private TextMeshProUGUI _tmpCost;
	[SerializeField] private UnfoldOnHover _hover;

	[Header("External references")]
	[SerializeField] private RSE_Sound _rsePlaySound;
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private AudioClip _onSelectedClip;

	[Header("Debugging")]
	public bool isSelected;
	public TileConfig tileConfig;

	public event Action<TileConfig> OnSelect;
	public event Action<TileConfig> OnUnselect;

	public void Initialize(TileConfig newTileConfig)
	{
		tileConfig = newTileConfig;
		Show();
		UpdateDisplays();
		Unselect();
	}

	private void UpdateDisplays()
	{
		_tmpTitle.text = tileConfig.tileName;
		_tmpFlavor.text = tileConfig.description;
		_imgIcon.sprite = tileConfig.icon;
		_imgBorder.color = tileConfig.borderColor;
		_tmpCost.text = $"{tileConfig.cost}";
	}

	public void ToggleSelection()
	{
		if (isSelected)
		{
			Unselect();
		}
		else
		{
			Select();
		}
	}

	private void Select()
	{
		OnSelect?.Invoke(tileConfig);
		_rsePlaySound.Call(TypeSound.SFX, _onSelectedClip, false);
		isSelected = true;
		HandleLock();
	}

	public void Unselect()
	{
		OnUnselect?.Invoke(tileConfig);
		_rsePlaySound.Call(TypeSound.SFX, _onSelectedClip, false);
		isSelected = false;
		HandleLock();
	}

	private void HandleLock()
	{
		if (tileConfig.cost >= _rsoHeart.value)
		{
			Lock();
		}
		else 
		{
			Unlock();
		}
	}

	private void Lock()
	{
		_goLock.SetActive(true);
	}

	private void Unlock()
	{
		_goLock.SetActive(false);
	}

	public void Fold()
	{
		_hover.Fold();
	}

	public void Show()
	{
		_goGraphics.SetActive(true);
	}

	public void Hide()
	{
		_goGraphics.SetActive(false);
	}
}