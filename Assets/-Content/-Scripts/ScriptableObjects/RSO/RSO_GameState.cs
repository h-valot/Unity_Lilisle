using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_GameState", menuName = "Data/RSO/Game state")]
public class RSO_GameState : RuntimeScriptableObject<GameState> { }

public enum GameState
{
	EDIT = 0,
	WAVE,
	GAME_OVER
}