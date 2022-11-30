using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPrayStoneEneity : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public PrayStoneSO prayStoneSO;

    private float moveCardSpeed = 0.2f;
    private Transform currentParent;
    private Vector3 currentPos;

    /// <summary>
    /// 生成该实体时初始化操作
    /// </summary>
    /// <param name="pray"></param>
    public void PrayStoneInit(PrayStoneSO pray)
    {
        prayStoneSO = pray;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentParent = transform.parent;
        currentPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(hit.point.x, 267f, hit.point.z);//待优化
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if (go != null)
        {
            if (go.tag.Contains("TerrainUI"))
            {
                SetDesktopCardPray(go.transform);
            }
            else if (go.GetComponent<MapDeskCardEneity>())
            {
                SwapParyStoneDesktopCard(transform,go.transform);
            }
            else
            {
                DOMoveTerrainCard(currentParent, currentPos);
            }
        }
        else
        {
            DOMoveTerrainCard(currentParent, currentPos);
        }
        MapSelectUIManager.Instance._mapBuffDetectionChannelSO.RaiseEvent();
    }

    /// <summary>
    /// 祈祷石拖入桌面卡牌
    /// </summary>
    /// <param name="target"></param>
    private void SetDesktopCardPray(Transform target)
    {
        GameObject desktopCrad = Instantiate(MapSelectUIManager.Instance.DesktopCradPrefab, target);
        desktopCrad.GetComponent<MapDeskCardEneity>().TerrainUIInit(prayStoneSO);
        target.GetComponent<MapTerrainUISlot>()._currentDesktipCard = desktopCrad.GetComponent<MapDeskCardEneity>();

        if (YKDataInfoManager.Instance.CurrentPrayStoneRecord.ContainsKey(prayStoneSO.PrayStonen_Type))
        {
            YKDataInfoManager.Instance.CurrentPrayStoneRecord[prayStoneSO.PrayStonen_Type] -= 1;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 祈祷石拖到桌面卡上面
    /// </summary>
    private void SwapParyStoneDesktopCard(Transform current, Transform target)
    {
        //1.判断桌面卡的祈祷石类型
        //2.增加对应祈祷石数量,销毁该桌面卡
        //3.在该桌面卡位置生成拖入的祈祷石卡牌
        //4.更新父物体的数据信息

        YKDataInfoManager.Instance.CurrentPrayStoneRecord[target.GetComponent<MapDeskCardEneity>()._prayStoneSO.PrayStonen_Type] += 1;
        
        GameObject desktopCrad = Instantiate(MapSelectUIManager.Instance.DesktopCradPrefab, target.parent);
        desktopCrad.GetComponent<MapDeskCardEneity>().TerrainUIInit(prayStoneSO);
        target.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = desktopCrad.GetComponent<MapDeskCardEneity>();

        if (YKDataInfoManager.Instance.CurrentPrayStoneRecord.ContainsKey(prayStoneSO.PrayStonen_Type))
        {
            YKDataInfoManager.Instance.CurrentPrayStoneRecord[prayStoneSO.PrayStonen_Type] -= 1;
        }

        switch (target.GetComponent<MapDeskCardEneity>()._prayStoneSO.PrayStonen_Type)
        {
            case PrayStoneType.Red:
                MapSelectUIManager.Instance.MapRedPrayStoneChangeChannel.RaiseEvent();
                break;
            case PrayStoneType.Green:
                MapSelectUIManager.Instance.MapGreenPrayStoneChangeChannel.RaiseEvent();
                break;
            case PrayStoneType.Blue:
                MapSelectUIManager.Instance.MapBluePrayStoneChangeChannel.RaiseEvent();
                break;
        }

        Destroy(target.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// 祈祷石归位
    /// </summary>
    /// <param name="target"></param>
    public void DOMoveTerrainCard(Transform target,Vector3 targetPos)
    {
        transform.SetParent(target);
        transform.DOMove(targetPos, moveCardSpeed);
    }
    public void DOMoveTerrainCard(Transform current, Transform target)
    {
        current.DOMove(target.position, moveCardSpeed);
        target.DOMove(currentParent.position, moveCardSpeed);

        current.parent.GetComponent<MapTerrainUISlot>()._currentTerrainUI = target.parent.GetComponent<MapTerrainUISlot>()._currentTerrainUI;

        current.SetParent(target.parent);
        target.SetParent(currentParent);
    }
}

