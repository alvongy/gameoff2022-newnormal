using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class GameLevelManager : SerializedMonoSingleton<GameLevelManager>
{
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "�ؿ�", ValueLabel = "�ؿ�����ʱ��")]
    public Dictionary<int, float> levelTimeConfigurationDic;
    [Title("�ؿ�ˢ��")]
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "�ؿ�ˢ��", ValueLabel = "ˢ��SO����")]
    public Dictionary<int, List<int>> spawnManagerConfigurationDic;
    [Title("�ؿ���ҵ���ϵ��")]
    [SerializeField][DictionaryDrawerSettings(KeyLabel = "�ؿ�", ValueLabel = "���˽�ҵ���ϵ��")]
    public Dictionary<int, float> levelGoldMultiplyDic;
    [Title("�ؿ�װ��ˢ�½�һ���")][SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "�ؿ�", ValueLabel = "װ��ˢ�½�һ���")]
    public Dictionary<int, int> levelGoldRefreshBasicDic;

    private int _currentLevel;
    private float _currenTime;
    private bool _isGamePlay;

    public int GetCurrentLevel => _currentLevel;
    public float GetCurrentTime => _currenTime;
    public bool IsGamePlay => _isGamePlay;

    public void Init()
    {
        Debug.Log("��Ϸ�ؿ���ʼ��");
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
    /// ��ǰ�ؿ������¼�
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
    /// ��һ���ؿ�����
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
