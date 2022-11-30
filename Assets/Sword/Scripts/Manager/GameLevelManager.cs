using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class GameLevelManager : SerializedMonoSingleton<GameLevelManager>
{
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "关卡", ValueLabel = "关卡持续时间")]
    public Dictionary<int, float> levelTimeConfigurationDic;
    [Title("关卡刷怪")]
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "关卡刷怪", ValueLabel = "刷怪SO配置")]
    public Dictionary<int, List<int>> spawnManagerConfigurationDic;
    [Title("关卡金币掉落系数")]
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "关卡", ValueLabel = "敌人金币掉落系数")]
    public Dictionary<int, float> levelGoldMultiplyDic;
    [Title("关卡装备刷新金币基数")][SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "关卡", ValueLabel = "装备刷新金币基数")]
    public Dictionary<int, int> levelGoldRefreshBasicDic;

    private int _currentLevel;
    private float _currenTime;
    private bool _isGamePlay;

    public int GetCurrentLevel => _currentLevel;
    public float GetCurrentTime => _currenTime;
    public bool IsGamePlay => _isGamePlay;

    public void Init()
    {
        Debug.Log("游戏关卡初始化");
        _currentLevel = 1;
        _currenTime = levelTimeConfigurationDic[_currentLevel];
        _isGamePlay = true;
    }

    public void OnGameOver()
    {
        _isGamePlay = false;
        UI_Manager.Instance.gameLevelPanel.ShowGameOverPopUI();
    }
    public void OnGameVictory()
    {
        _isGamePlay = false;
        UI_Manager.Instance.gameLevelPanel.ShowGameVictoryrPopUI();
    }

    private void Update()
    {
        if (!_isGamePlay || !EntranceManager.Instance.InGame) { return; }
        //GameTimeControl();
    }

    /// <summary>
    /// 当前关卡结束事件
    /// </summary>
    public void CurrentLevelEnd()
    {
        //_currentLevel++;
        if (_currentLevel < spawnManagerConfigurationDic.Count)
        {
            //_currenTime = levelTimeConfigurationDic[_currentLevel];

            _isGamePlay = false;
            MonsterManager.Instance.OnCurrentLevelEnd();
            SceneItemManager.Instance.OnCurrentLevelEnd();
            SpawnManager.Instance.OnCurrentLevelEnd();
            ShopManager.Instance.OnCurrentLevelEnd();
            EvolutionManager.Instance.OnCurrentLevelEnd();

            //Time.timeScale = 0;
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 1f, true);
        }
        else
        {
            CharacterManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// 下一波关卡继续
    /// </summary>
    public void NextLevelStart()
    {
        _isGamePlay = true;
        //Time.timeScale = 1;
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, 1f, 0.5f, true, 0.5f, true);

        _currentLevel++;
        EvolutionManager.Instance.OnGameResume();
        SpawnManager.Instance.OnLevelEnter(spawnManagerConfigurationDic[_currentLevel]);
        CharacterManager.Instance.ResumeMove();
    }
}
