using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquip_DragItem : DragableItem
{
    public Equip equip;
    public Image icon_Image;
    
    public override void DragEnd()
    {
        equip = null;
        icon_Image.enabled = false;
        dragTrigger.DragEnd(false);
    }

    public override void Dragging(bool canset, Vector2 pos)
    {
        transform.position = pos;
    }

    public override void DragStart(DragTrigger _dragTrigger)
    {
        icon_Image.sprite = equip.data.Icon;
        dragTrigger = _dragTrigger;
        icon_Image.enabled = true;
    }

    public override void OnDragCompelete(Vector2 pos)
    {
        equip = null;
        icon_Image.enabled = false;
        dragTrigger.DragEnd(true);
    }

    public override DragTarget SearchTarget(List<DragTarget> resultsList)
    {
        DragTarget result = null;
        float sqrMagnitude = float.MaxValue;
        for (int i = 0; i < resultsList.Count; i++)
        {
            UIEquip_DragTarget ui = resultsList[i] as UIEquip_DragTarget;
            if (ui)
            {
                float t = (resultsList[i].transform.position - transform.position).sqrMagnitude;
                if (ui.Part == equip.data.PartCode && t < sqrMagnitude)
                {
                    result = ui;
                    sqrMagnitude = t;
                }
            }
        }
        return result;
    }

    public override bool CheckTarget(DragTarget dragTarget)
    {
        UIEquip_DragTarget ui = dragTarget as UIEquip_DragTarget;
        return ui && ui.Part == equip.data.PartCode;
    }
}
