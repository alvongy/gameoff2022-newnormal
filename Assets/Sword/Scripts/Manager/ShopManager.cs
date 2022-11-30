using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class ShopManager : SerializedMonoSingleton<ShopManager>
{
    public PlayerAttributesSO playerGlods;
    public int goldPrice = 2;

    public void Init()
    {
        //初始化金币等
        playerGlods.ForceSetCurrentValue(10);
    }

    public void OnGameOver()
    {

    }

    /// <summary>
    /// 弹窗商店界面
    /// </summary>
    public void ShownShopPanel()
    {
        UI_Manager.Instance.shopControlPanel.ShowShopChoiceUI();
        UI_Manager.Instance.characterInfoCard.Show();
        UI_Manager.Instance.characterInfoCard.Refresh();
    }
    
    /// <summary>
    /// 商店购买完之后的事件
    /// </summary>
    public void EndPurchase()
    {
        UI_Manager.Instance.characterInfoCard.Hide();
        UI_Manager.Instance.shopControlPanel.HideShopChoiceUI();
        GameLevelManager.Instance.NextLevelStart();
    }

    /// <summary>
    /// 金币不足，购买失败
    /// </summary>
    public void ShowInsufficientGold()
    {
        UI_Manager.Instance.shopControlPanel.ShowInsufficientGoldPopUI();
    }

    /// <summary>
    /// 当前装备显示/隐藏
    /// </summary>
    /// <param name="active"></param>
    public void CurrentEquipInfoCardUIControl(Equip equip, bool active)
    {
        if (active)
        {
            UI_Manager.Instance.shopControlPanel.ShowCurrentEquippedInfoCard(equip);
        }
        else
        {
            UI_Manager.Instance.shopControlPanel.HideCurrentEquippedInfoCard();
        }
    }

    public void OnCurrentLevelEnd()
    {
        goldPrice = GameLevelManager.Instance.levelGoldRefreshBasicDic[GameLevelManager.Instance.GetCurrentLevel];
    }
}
