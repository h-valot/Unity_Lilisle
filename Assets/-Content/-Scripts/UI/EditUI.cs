using System.Collections.Generic;
using UnityEngine;

public class EditUI : MonoBehaviour
{
	[Header("Internal references")]
	[SerializeField] private GameObject _goHandParent;
	[SerializeField] private GameObject _goHand;
	[SerializeField] private GameObject _goHandGuide;
	[SerializeField] private GameObject _goPlacement;
	[SerializeField] private CardUI[] _cardsUI;

	[Header("External references")]
	[SerializeField] private RewardsConfig _rewardsConfig;
	[SerializeField] private RSO_GameState _rsoGameState;
	[SerializeField] private RSO_CurrentWave _rsoCurrentWave;
	[SerializeField] private RSO_Heart _rsoHeart;
	[SerializeField] private RSE_PlayNextWave _rsePlayNextWave;
	[SerializeField] private RSE_SetCursor _rseSetCursor;
	[SerializeField] private RSE_TilePlaced _rseTilePlaced;
	[SerializeField] private RSE_TilePlacementFailed _rseTilePlacementFailed;

	private TileConfig _selectedTile;
	private List<TileConfig> _hand;

	private void HandleEdit()
	{
		if (_rsoGameState.value == GameState.EDIT)
		{
			Initialize();
		}
		else 
		{
			HideCards();
			_goHandParent.SetActive(false);
			_goPlacement.SetActive(false);
		}
	}

	private void Initialize()
	{	
		_goHandParent.SetActive(true);
		_goPlacement.SetActive(false);
		HideCards();

		if (_rsoCurrentWave.value >= 0)
		{
			FillHand();
			_goHandGuide.SetActive(true);
			_goHand.SetActive(true);
			InitializeCards();
		}
	}

	private void FillHand()
	{
		_hand = new List<TileConfig>();
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_hand.Add(_rewardsConfig.tiles[Random.Range(0, _rewardsConfig.tiles.Length)]);
			_cardsUI[i].Initialize(_hand[i]);
		}
	}

	private void InitializeCards()
	{
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			if (_hand.Count > i)
			{
				_cardsUI[i].Initialize(_hand[i]);
			}
			else
			{
				_cardsUI[i].Hide();
			}
		}
	}

	public void PlayNextWave()
	{
		_rsePlayNextWave.Call();
	}

	private void EnterPlacementMode(TileConfig newTileConfig)
	{
		_selectedTile = newTileConfig;
		_rseSetCursor.Call(_selectedTile.tile);
		_goHandParent.SetActive(false);
		_goPlacement.SetActive(true);
	}

	public void ExitPlacementMode()
	{
		UnselectCards();
		_rseSetCursor.Call(null);
		_goHandParent.SetActive(true);
		_goPlacement.SetActive(false);
	}

	private void HandleTilePlaced()
	{
		_rsoHeart.value -= _selectedTile.cost;
		_hand.Remove(_selectedTile);
		InitializeCards();
		FoldCards();
		ExitPlacementMode();
	}

	private void FoldCards()
	{
		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].Fold();
		}
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
		_rseTilePlacementFailed.action += ExitPlacementMode;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect += EnterPlacementMode;
		}
	}

	private void OnDisable()
	{
		_rsoGameState.OnChanged -= HandleEdit;
		_rseTilePlaced.action -= HandleTilePlaced;
		_rseTilePlacementFailed.action -= ExitPlacementMode;

		for (int i = 0; i < _cardsUI.Length; i++)
		{
			_cardsUI[i].OnSelect -= EnterPlacementMode;
		}
	}
}