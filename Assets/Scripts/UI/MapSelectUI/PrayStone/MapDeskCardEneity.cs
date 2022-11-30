using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapDeskCardEneity : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, ICanvasRaycastFilter
{
    public PrayStoneSO _prayStoneSO;
    public Text _terrainName;
    public Image _terrainIcon;
    public bool _terrainIsOn;
    public float _moveCardSpeed = 0.2f;

    private Transform currentParent;
    private Transform tempParent;
    private MapDeskCardEneity currentDesktipCard;
    private bool isRaycastLocationValid = true;//默认射线不能穿透物体

    public void TerrainUIInit(PrayStoneSO pray)
    {
        _prayStoneSO = pray;
        _terrainName.text = pray.PrayStoneName.GetLocalizedString();
        _terrainIcon.sprite = pray.PrayStoneSprite;
        transform.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = this;
        tempParent = MapSelectUIManager.Instance.TerrainUITempParent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentParent = transform.parent;
        currentDesktipCard = transform.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard;
        isRaycastLocationValid = false;
        transform.parent = tempParent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.parent = currentParent;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
        }
        GameObject hitGO = hit.transform.gameObject;
        GameObject go = eventData.pointerCurrentRaycast.gameObject;

        //Debug.Log("鼠标松开检测到的UI对象："+go+"，物理对象："+ hitGO);

        if (go != null)
        {
            if (go.tag.Contains("TerrainUI"))
            {
                DOMoveTerrainCard(go.transform);
            }
            else if (go.GetComponent<MapDeskCardEneity>())
            {
                DOMoveSwapTerrainCard(go.transform);
            }
            else
            {
                DOMoveTerrainCard(currentParent);
            }
        }
        else if (hitGO.GetComponent<MapPrayStoneSlot>() && hitGO.GetComponent<MapPrayStoneSlot>()._prayStonen_Type == _prayStoneSO.PrayStonen_Type)
        {
            transform.parent = currentParent;
            SetDesktopCardPray(hitGO.transform);
        }
        else
        {
            DOMoveTerrainCard(currentParent);
        }

        isRaycastLocationValid = true;//射线不可以穿透物体
        MapSelectUIManager.Instance._mapBuffDetectionChannelSO.RaiseEvent();//发送地图加成检测事件
    }

    /// <summary>
    /// 桌面卡牌拖入祈祷石区域,进行销毁等操作，数据清理
    /// </summary>
    /// <param name="target"></param>
    private void SetDesktopCardPray(Transform target)
    {
        transform.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = null;
        transform.DOMove(target.position, _moveCardSpeed);

        if (YKDataInfoManager.Instance.CurrentPrayStoneRecord.ContainsKey(_prayStoneSO.PrayStonen_Type))
        {
            YKDataInfoManager.Instance.CurrentPrayStoneRecord[_prayStoneSO.PrayStonen_Type] += 1;
        }
        switch (_prayStoneSO.PrayStonen_Type)
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

        Destroy(gameObject);
    }

    public void DOMoveTerrainCard(Transform target)
    {
        transform.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = null;
        target.GetComponent<MapTerrainUISlot>()._currentDesktipCard = currentDesktipCard;

        transform.SetParent(target);
        transform.DOMove(target.position, _moveCardSpeed);
    }

    public void DOMoveSwapTerrainCard(Transform target)
    {
        transform.DOMove(target.position, _moveCardSpeed);
        target.DOMove(currentParent.position, _moveCardSpeed);

        transform.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = target.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard;
        target.parent.GetComponent<MapTerrainUISlot>()._currentDesktipCard = currentDesktipCard;

        transform.SetParent(target.parent);
        target.SetParent(currentParent);
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycastLocationValid;
    }
}
