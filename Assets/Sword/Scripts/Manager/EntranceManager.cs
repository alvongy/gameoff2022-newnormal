using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceManager : SerializedMonoSingleton<EntranceManager>
{
    public CharacterSO characterSO;
    [HideInInspector] public DegreeOfDifficultySO difficultySO;
    public bool InGame => inGame;
    private bool inGame = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        Application.targetFrameRate = 60;
        inGame = false;
        //StartGame();
    }

    public void StartGame()
    {
        CharacterManager.Instance.Init(characterSO);
        SceneItemManager.Instance.StartGame();
    }

    public void Play()
    {
        Destroy(CharacterManager.Instance.character.gameObject);

        CharacterManager.Instance.Init(characterSO);
        UI_Manager.Instance.Init(characterSO);
        DifficultyManager.Instance.Init();
        CaptureManager.Instance.Init(characterSO);
        SpawnManager.Instance.Init();
        ShopManager.Instance.Init();

        EvolutionManager.Instance.Init();
        GameLevelManager.Instance.Init();
        SceneItemManager.Instance.Init();
        inGame = true;
    }

    public void GameOver()
    {
        inGame = false;
        SpawnManager.Instance.OnGameOver();
        MonsterManager.Instance.OnGameOver();
        //UI_Manager.Instance.OnGameOver();
        BattleManager.Instance.OnGameOver();

        GameLevelManager.Instance.OnGameOver();
        EvolutionManager.Instance.OnGameOver();
        SceneItemManager.Instance.OnGameOver();
        UI_Manager.Instance.OnGameOver();
        Debug.Log("µØ¥∞”Œœ∑ ß∞‹Ω· ¯£°");
    }

    public void Victory()
    {
        inGame = false;
        SpawnManager.Instance.OnGameOver();
        MonsterManager.Instance.OnGameOver();
        //UI_Manager.Instance.OnGameOver();
        BattleManager.Instance.OnGameOver();
        //EvolutionManager.Instance.OnGameOver();
        SceneItemManager.Instance.OnGameOver();
        GameLevelManager.Instance.OnGameVictory();
    }

}
