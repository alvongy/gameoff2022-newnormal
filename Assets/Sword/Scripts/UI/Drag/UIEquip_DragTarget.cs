using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquip_DragTarget : DragTarget
{
    public EquipPart Part => part;
    private EquipPart part;

    public bool Equipped => item_UIEquip.Alive;
    [SerializeField] UIEquipItem item_UIEquip;
    [SerializeField] Image border_Image;
    [SerializeField] Text part_Text;

    public void Init(EquipPart _part)
    {
        part = _part;
        part_Text.text = CaptureManager.Instance.LocalizedMetaName_EquipPartDic[_part].GetLocalizedString();
    }

    public void Clear()
    {
        if (item_UIEquip.equip != null)
        {
            CaptureManager.Instance.DestroyEquip(item_UIEquip.equip);
        }
        item_UIEquip.equip = null;
        item_UIEquip.Hide();
    }

    public override void OnArrival(DragableItem dragableItem)
    {
        UIEquip_DragItem ui = dragableItem as UIEquip_DragItem;
        if (item_UIEquip.Alive)
        {
            CharacterManager.Instance.characterData.ArmEquip(ui.equip, item_UIEquip.equip);
            CaptureManager.Instance.DestroyEquip(item_UIEquip.equip);
            item_UIEquip.equip = null;
        }
        else
        {
            CharacterManager.Instance.characterData.ArmEquip(ui.equip);
        }
        UI_Manager.Instance.captureEquipPanel.ShowArrow(false);
        item_UIEquip.Init(ui.equip);

    }

    public override void OnDragEnter()
    {
        CompareEquip();
        border_Image.color = Color.yellow;
    }

    public void CompareEquip()
    {
        if (item_UIEquip.Alive)
        {
            //显示现有装备信息
            UI_Manager.Instance.captureEquipPanel.equippedInfoCard.Show(item_UIEquip.equip);
        }
        else
        {
            UI_Manager.Instance.captureEquipPanel.equippedInfoCard.ShowEmpty();
        }
        UI_Manager.Instance.captureEquipPanel.ShowArrow(true);
    }

    public override void OnDragExit()
    {
        UI_Manager.Instance.captureEquipPanel.equippedInfoCard.Hide();
        UI_Manager.Instance.captureEquipPanel.ShowArrow(false);
        border_Image.color = Color.white;
    }

    public override void OnReady()
    {
        //当前拖拽物能够拖入时的效果
        //border_Image.color = Color.yellow;
    }

    public override void OnMiss()
    {
        //关闭Ready效果
        //border_Image.color = Color.white;
    }
}
