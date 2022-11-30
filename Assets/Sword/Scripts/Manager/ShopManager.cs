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
        //��ʼ����ҵ�
        playerGlods.ForceSetCurrentValue(10);
    }

    public void OnGameOver()
    {

    }

    /// <summary>
    /// �����̵����
    /// </summary>
    public void ShownShopPanel()
    {
        UI_Manager.Instance.shopControlPanel.ShowShopChoiceUI();
        UI_Manager.Instance.characterInfoCard.Show();
        UI_Manager.Instance.characterInfoCard.Refresh();
    }
    
    /// <summary>
    /// �̵깺����֮����¼�
    /// </summary>
    public void EndPurchase()
    {
        UI_Manager.Instance.characterInfoCard.Hide();
        UI_Manager.Instance.shopControlPanel.HideShopChoiceUI();
        GameLevelManager.Instance.NextLevelStart();
    }

    /// <summary>
    /// ��Ҳ��㣬����ʧ��
    /// </summary>
    public void ShowInsufficientGold()
    {
        UI_Manager.Instance.shopControlPanel.ShowInsufficientGoldPopUI();
    }

    /// <summary>
    /// ��ǰװ����ʾ/����
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
