using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapQuestPanelControl : MonoBehaviour
{
    [SerializeField] public TerrainEventChannelSO OnPickTerrain;
    public GameObject _terrainCardObj;
    public Transform _mapQuestParent;

    public Button _enterTerrain_Btn;
    public Button _Conceal_Btn;

    private int currentTerrainID;
    private Transform questTransform;

    private void Start()
    {
        _enterTerrain_Btn.onClick.AddListener(EnterTerrain);
        _Conceal_Btn.onClick.AddListener(ConcealPanel);
        
    }
    private void OnEnable()
    {
        OnPickTerrain.OnEventRaised += StartSelect;
    }
    private void OnDisable()
    {
        OnPickTerrain.OnEventRaised -= StartSelect;
    }

    public void InitFirstQuest()
    {
        OnPickTerrain.RaiseEvent(_mapQuestParent.GetChild(0).GetComponent<MapQuestEneity>()._currentTerrainSO);
    }

    private void StartSelect(TerrainSO ter)
    {
        currentTerrainID = ter.TID;
        RefreshSelectTerrain();
    }

    void EnterTerrain()
    {
        MapSelectUIManager.Instance.LoadScene.Load();//进入选择的地形卡任务
        YKRewardWatcher.Instance.TerrainStorer.Remove(currentTerrainID);//将选择过的任务剔除

        for (int i = 0; i < MapSelectUIManager.Instance.TerrainUIParent.childCount; i++)
        {
            if (MapSelectUIManager.Instance.TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard != null)
            {
                if (!YKDataInfoManager.Instance.CurrentDesktopGridRecord.ContainsKey(i))
                {
                    YKDataInfoManager.Instance.CurrentDesktopGridRecord.Add(i, MapSelectUIManager.Instance.TerrainUIParent.GetChild(i).GetComponent<MapTerrainUISlot>()._currentDesktipCard._prayStoneSO);//进入游戏场景时将桌面卡牌位置保存
                }
            }
        }
    }

    void ConcealPanel()
    {
        gameObject.SetActive(false);
    }

    void RefreshSelectTerrain()
    {
        for (int i = 0; i < _mapQuestParent.childCount; i++)
        {
            questTransform = _mapQuestParent.GetChild(i);
            if (questTransform.GetComponent<MapQuestEneity>().TerrainID == currentTerrainID)
            {
                questTransform.GetComponent<MapQuestEneity>()._selectFlag.SetActive(true);
                //questTransform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                questTransform.GetComponent<MapQuestEneity>()._selectFlag.SetActive(false);
                //questTransform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
