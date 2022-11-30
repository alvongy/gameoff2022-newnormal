using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum GameState
{
    Gameplay, //regular state: player moves, attacks, can perform actions
    Pause, //pause menu is opened, the whole game world is frozen
    Inventory, //when inventory UI or cooking UI are open
    Dialogue,
    Cutscene,
}
//[CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 51)]
public class GameStateSO : DescriptionBaseSO
{
    public GameState CurrentGameState => _currentGameState;

    [Header("Game states")]
    [SerializeField] [ReadOnlyInspector] private GameState _currentGameState = default;
    [SerializeField] [ReadOnlyInspector] private GameState _previousGameState = default;


    private void Start()
    {
    }
	public void UpdateGameState(GameState newGameState)
	{
		if (newGameState == CurrentGameState)
			return;


		_previousGameState = _currentGameState;
		_currentGameState = newGameState;
	}
}
