using System.Collections;
using UnityEngine;

public class EditUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goHand;
	[SerializeField] private GameObject _goPlacement;
	[SerializeField] private CardUI[] _cardsUI;

	[Header("External references")]
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_HandCard _rsoHandCard;
	[SerializeField] private RSE_PlayNextWave _rsePlayNextWave;
	[SerializeField] private RSE_HideHUD _rseHideHUD;
	[SerializeField] private RSE_SetCursor _rseSetCursor;
	[SerializeField] private RSE_TilePlaced _rseTilePlaced;


	[Header("Tweakable values")]
	[SerializeField] private float _cardInitializationDelay = 0.2f;

	private TileConfig _selectedTile;

	private void HandleEdit()
	{
		if (_rsoGameState.value == GameState.EDIT)
		{
			Initialize();
		}
		else 
		{
			HideCards();
			_goHand.SetActive(false);
			_goPlacement.SetActive(false);
		}
	}

	public void PlayNextWave()
	{
		_rsePlayNextWave.Call();
		_rsoHandCard.value.Clear();
	}

	private void Initialize()
	{	
		_goHand.SetActive(true);
		_goPlacement.SetActive(false);
		StartCoroutine(FillHand());
	}

	private IEnumerator FillHand()
	{
		HideCards();
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			if (_rsoHandCard.value.Count > i)
			{
				_cardsUI[i].Initialize(_rsoHandCard.value[i]);
			}
			else
			{
				_cardsUI[i].Hide();
			}
			yield return new WaitForSeconds(_cardInitializationDelay);
		}
	}

	private void EnterPlacementMode(TileConfig newTileConfig)
	{
		_selectedTile = newTileConfig;
		_rseSetCursor.Call(_selectedTile.tile);
		_rseHideHUD.Call(true);
		_goHand.SetActive(false);
		_goPlacement.SetActive(true);
	}

	public void ExitPlacementMode()
	{
		UnselectCards();
		_rseSetCursor.Call(null);
		_rseHideHUD.Call(false);
		_goHand.SetActive(true);
		_goPlacement.SetActive(false);
	}

	private void HandleTilePlaced()
	{
		_rsoHandCard.value.Remove(_selectedTile);
		StartCoroutine(FillHand());
		ExitPlacementMode();
	}

	private void HideCards()
	{
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].Hide();
		}
	}

	private void UnselectCards()
	{
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].Unselect();
		}
	}

	private void OnEnable()
	{
		_rsoGameState.OnChanged += HandleEdit;
		_rseTilePlaced.action += HandleTilePlaced;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect += EnterPlacementMode;
		}
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleEdit;
		_rseTilePlaced.action -= HandleTilePlaced;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect -= EnterPlacementMode;
		}
	}
}
