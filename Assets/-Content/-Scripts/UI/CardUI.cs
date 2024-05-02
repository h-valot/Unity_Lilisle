using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private TextMeshProUGUI _tmpTitle;
	[SerializeField] private TextMeshProUGUI _tmpFlavor;
	[SerializeField] private TextMeshProUGUI _tmpCost;
	[SerializeField] private Image _imgIcon;

	[Header("External references")]
	[SerializeField] private RSO_HandCard _rsoHandCard;

	[Header("Debugging")]
	public bool isSelected;
	public TileConfig tileConfig;

	public void Initialize(TileConfig newTileConfig)
	{
		tileConfig = newTileConfig;
		UpdateDisplays();
		Unselect();
	}

	private void UpdateDisplays()
	{
		_tmpTitle.text = tileConfig.tileName;
		_tmpFlavor.text = tileConfig.description;
		//_imgIcon.sprite = card.tile.icon;
		_tmpCost.text = $"Cost: lose {tileConfig.cost} hearts";
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
		print("card added to hand");
		_rsoHandCard.value.Add(tileConfig);
		isSelected = true;
	}

	private void Unselect()
	{
		print("card removed from hand");
		_rsoHandCard.value.Remove(tileConfig);
		isSelected = false;
	}
}