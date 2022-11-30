using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectUIManager : MonoSingleton<MapSelectUIManager>
{
    [FoldoutGroup("TerrainSeclect")]
    [InfoBox("��ͼѡ�񲿷�")]
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
    //[FoldoutGroup("TerrainSeclect")][InfoBox("Ĭ�ϵ�һ�����Ƶ���������")]
    //public TerrainSO firstTerrain;
    //[FoldoutGroup("TerrainSeclect")]
    //[InfoBox("Ĭ�ϵڶ������Ƶ���������")]
    //public TerrainSO secondTerrain;
    //[FoldoutGroup("TerrainSeclect")]
    //[InfoBox("Ĭ�ϵ��������Ƶ���������")]
    //public TerrainSO thirdTerrain;
    //[FoldoutGroup("TerrainSeclect")][InfoBox("Ĭ��Boss���Ƶ���������")]
    //public TerrainSO bossTerrain;

    [FoldoutGroup("TerrainSeclect")][InfoBox("���濨�Ƶ�����")]
    public MapBuffControl _mapBuffControl;

    [FoldoutGroup("PlayerDataPanel")]
    [InfoBox("������ݲ���")]
    public MapPlayerBasicData PlayerBasicData;

    [FoldoutGroup("MapDataPanel")]
    [InfoBox("�������ݲ���")]
    [FoldoutGroup("MapDataPanel")]
    public Button EnterGame_Btn;
    [FoldoutGroup("MapDataPanel")]
    public MainMenuLoadScene LoadScene;
    [FoldoutGroup("MapDataPanel")]
    public MainMenuLoadScene LoadMenuScene;
    [FoldoutGroup("MapDataPanel")]
    public bool IsActiveEnterGame_Btn = true;

    [FoldoutGroup("MapQuestPanel")][InfoBox("���ο�����")]
    public GameObject MapQuestPanel;
    [FoldoutGroup("MapQuestPanel")]
    [InfoBox("ʤ������_��ɱBoss������")]
    public TerrainSO BossTerrainSO;

    [FoldoutGroup("��ʯ����")]
    public GameObject PrayStonePrefab;
    [FoldoutGroup("��ʯ����")]
    //public VoidEventChannelSO MapPrayStoneChangeChannel;
    public VoidEventChannelSO MapRedPrayStoneChangeChannel;
    [FoldoutGroup("��ʯ����")]
    public VoidEventChannelSO MapGreenPrayStoneChangeChannel;
    [FoldoutGroup("��ʯ����")]
    public VoidEventChannelSO MapBluePrayStoneChangeChannel;

    [FoldoutGroup("��ͼ��������")][InfoBox("������õļ���ѡ�����")]
    [SerializeField]public VoidEventChannelSO _selectSkillChoicePanelEvent;

    [FoldoutGroup("��ͼ��������")][InfoBox("������õĽ�������")]
    [SerializeField] public VoidEventChannelSO _terrainRawardPanelEvent;

    [FoldoutGroup("��ͼ��������")]
    public MapPopPanelControl PopUpPanel;
    [FoldoutGroup("��ͼ��������")]
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
            //_selectSkillChoicePanelEvent.RaiseEvent();//����ѡ�񵯴�
            _terrainRawardPanelEvent.RaiseEvent();//��ͼ��������
            OpenRawardPanel();
        }
    }
    /// <summary>
    /// ��ʼ�����泡����һЩ���ݡ��磺������Ƿ����������֮ǰ����Ŀ��Ƽ�λ��
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

        if (YKDataInfoManager.Instance._isPlayerDie) //��������������Ҫ�������黹
        { 
            YKDataInfoManager.Instance._energySO.Increace(-YKDataInfoManager.Instance._energyTempNum);
            //_mapBuffControl.PlayerDiedPunishment();
        }
    }
    /// <summary>
    /// ����֮ǰ��������濨��
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
        YKDataInfoManager.Instance.CurrentDesktopGridRecord.Clear();//��������κ���մ���ĵ���
    }

    /// <summary>
    /// ����������Ľ�������
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
            // Debug.Log("��һ�½�������ִ�в�����");
            PopUpPanel.InitializeGameOver();
            return;
        }

        //������������
        PopUpPanel.InitializeTerrainRaward(TerrainAttribute.value.GetQuests()[0].Rewards[0]);
    }

    /// <summary>
    /// ���������ĳͷ�
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

    #region  ��������۳�һ���Ƶĳͷ���ʱ�۳�
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
        //RandomDestructionTer.Remove(firstTerrain.TID);//��һ���Ʋ���Ϊ���ٿ���

        int ran = UnityEngine.Random.Range(0, RandomDestructionTer.Count);
        Debug.Log("���Կ۳��Ŀ��ƣ�ran:"+ran+ "��[RandomDestructionTer[ran]��"+RandomDestructionTer[ran]);
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
    /// ����ѡ����Ϻ�ִ��
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
                    YKDataInfoManager.Instance.CurrentDesktopGridRecord.Add(i, TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard._prayStoneSO);//������Ϸ����ʱ�����濨��λ�ñ���
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
