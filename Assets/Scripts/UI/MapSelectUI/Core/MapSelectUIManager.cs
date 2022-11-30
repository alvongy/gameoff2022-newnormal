using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectUIManager : MonoSingleton<MapSelectUIManager>
{
    [FoldoutGroup("TerrainSeclect")]
    [InfoBox("地图选择部分")]
    [SerializeField] public TerrainDataBase _terrainDataBase;
    [FoldoutGroup("TerrainSeclect")]
    [SerializeField] public TerrainEventChannelSO OnPickTerrain;
    [FoldoutGroup("TerrainSeclect")]
    [SerializeField] TerrainAttibuteSO TerrainAttribute;
    [FoldoutGroup("TerrainSeclect")]
    [SerializeField] public VoidEventChannelSO _mapBuffDetectionChannelSO;
    [FoldoutGroup("TerrainSeclect")]
    public GameObject MapDataPanel;
    [FoldoutGroup("TerrainSeclect")]
    public GameObject MapBuffPanel;
    [FoldoutGroup("TerrainSeclect")]
    public GameObject DesktopCradPrefab;
    [FoldoutGroup("TerrainSeclect")]
    public GameObject DesktopGirdLockPrefab;
    [FoldoutGroup("TerrainSeclect")]
    public Transform TerrainUIParent;
    [FoldoutGroup("TerrainSeclect")]
    public Transform TerrainUITempParent;
   // [FoldoutGroup("TerrainSeclect")]
    //private List<Transform> _terrainUIList=new List<Transform>();
    //[FoldoutGroup("TerrainSeclect")][InfoBox("默认第一个卡牌的属性配置")]
    //public TerrainSO firstTerrain;
    //[FoldoutGroup("TerrainSeclect")]
    //[InfoBox("默认第二个卡牌的属性配置")]
    //public TerrainSO secondTerrain;
    //[FoldoutGroup("TerrainSeclect")]
    //[InfoBox("默认第三个卡牌的属性配置")]
    //public TerrainSO thirdTerrain;
    //[FoldoutGroup("TerrainSeclect")][InfoBox("默认Boss卡牌的属性配置")]
    //public TerrainSO bossTerrain;

    [FoldoutGroup("TerrainSeclect")][InfoBox("桌面卡牌的配置")]
    public MapBuffControl _mapBuffControl;

    [FoldoutGroup("PlayerDataPanel")]
    [InfoBox("玩家数据部分")]
    public MapPlayerBasicData PlayerBasicData;

    [FoldoutGroup("MapDataPanel")]
    [InfoBox("地形数据部分")]
    [FoldoutGroup("MapDataPanel")]
    public Button EnterGame_Btn;
    [FoldoutGroup("MapDataPanel")]
    public MainMenuLoadScene LoadScene;
    [FoldoutGroup("MapDataPanel")]
    public MainMenuLoadScene LoadMenuScene;
    [FoldoutGroup("MapDataPanel")]
    public bool IsActiveEnterGame_Btn = true;

    [FoldoutGroup("MapQuestPanel")][InfoBox("地形卡任务")]
    public GameObject MapQuestPanel;
    [FoldoutGroup("MapQuestPanel")]
    [InfoBox("胜利条件_击杀Boss的任务")]
    public TerrainSO BossTerrainSO;

    [FoldoutGroup("祈祷石部分")]
    public GameObject PrayStonePrefab;
    [FoldoutGroup("祈祷石部分")]
    //public VoidEventChannelSO MapPrayStoneChangeChannel;
    public VoidEventChannelSO MapRedPrayStoneChangeChannel;
    [FoldoutGroup("祈祷石部分")]
    public VoidEventChannelSO MapGreenPrayStoneChangeChannel;
    [FoldoutGroup("祈祷石部分")]
    public VoidEventChannelSO MapBluePrayStoneChangeChannel;

    [FoldoutGroup("地图弹窗部分")][InfoBox("弹出获得的技能选择界面")]
    [SerializeField]public VoidEventChannelSO _selectSkillChoicePanelEvent;

    [FoldoutGroup("地图弹窗部分")][InfoBox("弹出获得的奖励界面")]
    [SerializeField] public VoidEventChannelSO _terrainRawardPanelEvent;

    [FoldoutGroup("地图弹窗部分")]
    public MapPopPanelControl PopUpPanel;
    [FoldoutGroup("地图弹窗部分")]
    public Button GameOver_Btn;

    private TerrainSO _currentTerrainSO;
    public List<int> RandomDestructionTer = new List<int>();

    private void OnEnable()
    {
        OnPickTerrain.OnEventRaised += TerrainSelect;
    }
    private void OnDisable()
    {
        OnPickTerrain.OnEventRaised -= TerrainSelect;
    }
    private void Awake()
    {
        MapSceneInit();
    }
    private void Start()
    {
        if (YKDataInfoManager.Instance.isFirstTerrain)
        {
            YKDataInfoManager.Instance.isFirstTerrain = false;
        }
        else
        {
            //_selectSkillChoicePanelEvent.RaiseEvent();//技能选择弹窗
            _terrainRawardPanelEvent.RaiseEvent();//地图奖励弹窗
            OpenRawardPanel();
        }
    }
    /// <summary>
    /// 初始化桌面场景的一些数据。如：检查标记是否解锁，加载之前保存的卡牌及位置
    /// </summary>
    private void MapSceneInit()
    {
        YKRewardWatcher.Instance.storer.ClearList();
        YKRewardWatcher.Instance.ReceiveRewards();
        YKDataInfoManager.Instance.IsClickedDesktopGrid = false;
        YKDataInfoManager.Instance.currentDesktopGridID = 10;

        MapBuffPanel.SetActive(true);
        TerrainUIParent.gameObject.SetActive(true);

        EnterGame_Btn.onClick.AddListener(EnterTerrainGame);
        GameOver_Btn.onClick.AddListener(()=> { LoadScene.RefreshIsFirstTerrain(); });

        for (int i = 0; i < TerrainUIParent.childCount; i++)
        {
            if (!YKDataInfoManager.Instance.DesktopGridLockRecord[i])
            {
                Instantiate(DesktopGirdLockPrefab, TerrainUIParent.GetChild(i));
            }
        }   

        LoadTerrainRecord();

        if (YKDataInfoManager.Instance._isPlayerDie) //玩家如果死亡，需要将能量归还
        { 
            YKDataInfoManager.Instance._energySO.Increace(-YKDataInfoManager.Instance._energyTempNum);
            //_mapBuffControl.PlayerDiedPunishment();
        }
    }
    /// <summary>
    /// 加载之前保存的桌面卡牌
    /// </summary>
    private void LoadTerrainRecord()
    {
        if (YKDataInfoManager.Instance == null) { return; }
        
        for (int i = 0; i < TerrainUIParent.childCount; i++)
        {
            if (TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard == null)
            {
                if (YKDataInfoManager.Instance.CurrentDesktopGridRecord.ContainsKey(i))
                {
                    GameObject desktopCrad = Instantiate(DesktopCradPrefab, TerrainUIParent.GetChild(i));
                    desktopCrad.GetComponent<MapDeskCardEneity>().TerrainUIInit(YKDataInfoManager.Instance.CurrentDesktopGridRecord[i]);
                }
            }
        }
        //PlayerDied();
        YKDataInfoManager.Instance.CurrentDesktopGridRecord.Clear();//加载完地形后清空储存的地形
    }

    /// <summary>
    /// 打开完成任务后的奖励界面
    /// </summary>
    private void OpenRawardPanel()
    {
        foreach (var q in BossTerrainSO.GetQuests())
        {
            if (YKQuestWatcher.Instance.questData.ContainsKey(q.QID) && YKQuestWatcher.Instance.questData[q.QID].state == YKQuestWatcher.QuestStatus.success)
            {
                YKDataInfoManager.Instance.isChapterOver = true;
            }
        }
        if (YKDataInfoManager.Instance.isChapterOver)
        {
            // Debug.Log("第一章结束，待执行操作！");
            PopUpPanel.InitializeGameOver();
            return;
        }

        //触发奖励弹窗
        PopUpPanel.InitializeTerrainRaward(TerrainAttribute.value.GetQuests()[0].Rewards[0]);
    }

    /// <summary>
    /// 玩家死亡后的惩罚
    /// </summary>
    void PlayerDied()
    {
        if (YKDataInfoManager.Instance._isPlayerDie)
        {
            //IsActiveEnterGame_Btn = true;
            //YKDataInfoManager.Instance.RefreshIsFirstTerrain();
            //LoadMenuScene.Load();
        }

        // PlayerDie();
    }

    #region  玩家死亡扣除一张牌的惩罚暂时扣除
    /*
    void PlayerDie()
    {
        ///
        if (YKDataInfoManager.Instance.CurrentDesktopGridRecord.Count <= 3 || !YKDataInfoManager.Instance._isPlayerDie) { return; }
        foreach (var t in YKDataInfoManager.Instance.CurrentDesktopGridRecord)
        {
            foreach (var q in t.Value.GetQuests())
            {
                if (YKQuestWatcher.Instance.questData.ContainsKey(q.QID) && YKQuestWatcher.Instance.questData[q.QID].state == YKQuestWatcher.QuestStatus.success)
                {
                    RandomDestructionTer.Add(t.Value.TID);
                }
            }
        }
       // if (RandomDestructionTer.Count <= 0) { return; }
        //foreach (var t in YKRewardWatcher.Instance.terrainRecordDic)
        //{
        //    if (YKQuestWatcher.Instance.questData.ContainsKey(t.Value.GetQuests()[0].QID) && YKQuestWatcher.Instance.questData[t.Value.GetQuests()[0].QID].state == YKQuestWatcher.QuestStatus.success)
        //    {
        //        RandomDestructionTer.Add(t.Value.TID);
        //    }
        //}
        //RandomDestructionTer.Remove(firstTerrain.TID);//第一张牌不作为销毁卡牌

        int ran = UnityEngine.Random.Range(0, RandomDestructionTer.Count);
        Debug.Log("测试扣除的卡牌：ran:"+ran+ "，[RandomDestructionTer[ran]："+RandomDestructionTer[ran]);
        PopUpPanel.InitializePlayerDiedData(_terrainDataBase.database[RandomDestructionTer[ran]]);

        YKRewardWatcher.Instance._terrainDataBaseSelected.database.Remove(RandomDestructionTer[ran]);

        int terKey = 0;
        foreach (var t in YKDataInfoManager.Instance.CurrentDesktopGridRecord)
        {
            if (t.Value.TID == RandomDestructionTer[ran])
            {
                terKey = t.Key;
            }
        }
        YKDataInfoManager.Instance.CurrentDesktopGridRecord.Remove(terKey);

        foreach (var q in _terrainDataBase.database[RandomDestructionTer[ran]].GetQuests())
        {
            if (YKQuestWatcher.Instance.questData.ContainsKey(q.QID))
            {
                //YKQuestWatcher.Instance.questData[q.QID].state = YKQuestWatcher.QuestStatus.ready;
                YKQuestWatcher.Instance.questData.Remove(q.QID);
            }

        }
        if (YKRewardWatcher.Instance.TerrainStorer.ContainsKey(RandomDestructionTer[ran])&& _terrainDataBase.database.ContainsKey(RandomDestructionTer[ran]))
        {
            YKRewardWatcher.Instance.TerrainStorer.Add(RandomDestructionTer[ran], _terrainDataBase.database[RandomDestructionTer[ran]]);
        }
    }
    */
    #endregion

    /// <summary>
    /// 地形选择完毕后执行
    /// </summary>
    private void TerrainSelect(TerrainSO terrain)
    {
        _currentTerrainSO = terrain;

        //MapBuffPanel.SetActive(false);
        MapDataPanel.SetActive(true);
        TerrainAttribute.value = terrain;
    }

    private void EnterTerrainGame()
    {
        LoadScene.Load();

        //if (!YKRewardWatcher.Instance._terrainDataBaseSelected.database.ContainsKey(firstTerrain.TID))
        //{
        //    YKRewardWatcher.Instance._terrainDataBaseSelected.database.Add(firstTerrain.TID, firstTerrain);
        //}
        for (int i = 0; i < TerrainUIParent.childCount; i++)
        {
            if (TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard != null)
            {
                if (!YKDataInfoManager.Instance.CurrentDesktopGridRecord.ContainsKey(i))
                {
                    YKDataInfoManager.Instance.CurrentDesktopGridRecord.Add(i, TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard._prayStoneSO);//进入游戏场景时将桌面卡牌位置保存
                }
            }
        }
     }   

    public void CheckTerrainBuff()
    {
        //MapBuffPanel.SetActive(true);
        MapDataPanel.SetActive(false);
    }

}
