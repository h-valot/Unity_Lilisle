using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goParent;
	[SerializeField] private TextMeshProUGUI _tmpTitle;
	[SerializeField] private TextMeshProUGUI _tmpFlavor;
	[SerializeField] private TextMeshProUGUI _tmpCost;
	[SerializeField] private UnfoldOnHover _hover;
	[SerializeField] private Image _imgIcon;

	[Header("External references")]
	[SerializeField] private RSE_Sound _rsePlaySound;
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
	}

	public void Unselect()
	{
		OnUnselect?.Invoke(tileConfig);
		_rsePlaySound.Call(TypeSound.SFX, _onSelectedClip, false);
		isSelected = false;
	}

	public void Fold()
	{
		_hover.Fold();
	}

	public void Show()
	{
		_goParent.SetActive(true);
	}

	public void Hide()
	{
		_goParent.SetActive(false);
	}
}