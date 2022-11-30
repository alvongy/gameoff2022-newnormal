using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DesktopGirdLock : MonoBehaviour, IPointerClickHandler
{
    private Transform questParent;
    private bool isFirstClick = true;
    private Dictionary<int, TerrainSO> mapQuestDic = new Dictionary<int, TerrainSO>();

    void OnEnable()
    {
        questParent = MapSelectUIManager.Instance.MapQuestPanel.GetComponent<MapQuestPanelControl>()._mapQuestParent;
        mapQuestDic = YKRewardWatcher.Instance.TerrainStorer;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (YKDataInfoManager.Instance.IsClickedDesktopGrid && YKDataInfoManager.Instance.currentDesktopGridID != transform.parent.GetComponent<MapTerrainUISlot>()._slotID) 
        { 
            return; 
        }
        YKDataInfoManager.Instance.currentDesktopGridID = transform.parent.GetComponent<MapTerrainUISlot>()._slotID;
        YKDataInfoManager.Instance.IsClickedDesktopGrid = true;
        transform.GetComponent<Image>().color = new Color(
            transform.GetComponent<Image>().color.r, transform.GetComponent<Image>().color.g, transform.GetComponent<Image>().color.b, 1);

        //从任务库中抽取三个任务显示到 任务UI中
        if (isFirstClick)
        {
            isFirstClick = false;
            MapSelectUIManager.Instance.MapQuestPanel.SetActive(true);
            GetMapQuest();
            //StartCoroutine(GetMapQuestS());
        }
        else
        {
            MapSelectUIManager.Instance.MapQuestPanel.SetActive(true);
        }
    }

    IEnumerator GetMapQuestS()
    {
        MapSelectUIManager.Instance.MapQuestPanel.SetActive(true);

        TerrainSO[] result = new TerrainSO[3];
        List<int> list = new List<int>();
        list.AddRange(mapQuestDic.Keys);
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, list.Count);
            result[i] = mapQuestDic[list[index]];
            list.RemoveAt(index);
        }

        for (int i = 0; i < result.Length; i++)
        {
            questParent.GetChild(i).GetComponent<MapQuestEneity>().MapQuestInit(result[i]);
        }
        MapSelectUIManager.Instance.MapQuestPanel.GetComponent<MapQuestPanelControl>().InitFirstQuest();
        
        yield return null;
    }
    void GetMapQuest()
    {
        TerrainSO[] result = new TerrainSO[3];
        List<int> list = new List<int>();
        list.AddRange(mapQuestDic.Keys);
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, list.Count);
            result[i] = mapQuestDic[list[index]];
            list.RemoveAt(index);
        }

        for (int i = 0; i < result.Length; i++)
        {
            questParent.GetChild(i).GetComponent<MapQuestEneity>().MapQuestInit(result[i]);
        }
        MapSelectUIManager.Instance.MapQuestPanel.GetComponent<MapQuestPanelControl>().InitFirstQuest();

        //if (mapQuestDic.Count == 0)
        //{
        //    return;
        //}
        //else if (mapQuestDic.Count <= 3)
        //{
        //    foreach (var r in mapQuestDic)
        //    {
        //        GameObject terrain = Instantiate(terrainCardObj, questParent);
        //        terrain.GetComponent<MapQuestEneity>().MapQuestInit(r.Value);
        //    }
        //}
        //else
        //{
        //    TerrainSO[] result = new TerrainSO[3];
        //    List<int> list = new List<int>();
        //    list.AddRange(mapQuestDic.Keys);
        //    for (int i = 0; i < 3; i++)
        //    {
        //        int index = Random.Range(0, list.Count);
        //        result[i] = mapQuestDic[list[index]];
        //        list.RemoveAt(index);
        //    }
        //
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        GameObject terrain = Instantiate(terrainCardObj, questParent);
        //        terrain.GetComponent<MapQuestEneity>().MapQuestInit(result[i]);
        //    }
        //}

    }
}
