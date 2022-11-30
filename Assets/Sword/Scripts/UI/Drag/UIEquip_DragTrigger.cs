using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquip_DragTrigger : DragTrigger
{
    [SerializeField] UIEquipItem equipItem;
    [SerializeField] CanvasGroup canvasGroup;

    public override DragableItem DragStart()
    {
        canvasGroup.alpha = 0.5f;
        UI_Manager.Instance.dragItem_Equip.equip = equipItem.equip;
        UI_Manager.Instance.captureEquipPanel.OnDragStartEquipPart(equipItem.equip);
        return UI_Manager.Instance.dragItem_Equip;
    }

    public override void DragEnd(bool success)
    {
        canvasGroup.alpha = 1f;
        UI_Manager.Instance.captureEquipPanel.OnDragEndEquipPart(equipItem.equip.data.PartCode);
        if (success)
        {
            UI_Manager.Instance.captureEquipPanel.PutBack(gameObject);
        }
    }
}
