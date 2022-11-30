using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopControl : MonoBehaviour
{
    [SerializeField] private Button nextWaveBtn;
    [SerializeField] private Button refreshEquipBtn;
    [SerializeField] private Button closePopBtn;
    [SerializeField] private Button captureGoldBtn;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Text refreshEquipText;

    [SerializeField] private Transform shopPanel;
    [SerializeField] private Transform UIEquipItemPool;
    [SerializeField] private Transform UISideEquipCapture;
    [SerializeField] private Transform UIInsufficientGoldPop;
    [SerializeField] private UICurrentEquipInfoCard currentEquippedInfoCard;

    [SerializeField] private GameObject EquipItem_Prefab;


    public void Init()
    {
        while (UISideEquipCapture.childCount > 0)
        {
            PutBack(UISideEquipCapture.GetChild(UISideEquipCapture.childCount - 1).gameObject);
        }
    }

    private void Awake()
    {
        RegisteredEvents();
    }

    private void RegisteredEvents()
    {
        nextWaveBtn.onClick.AddListener(NextWave);
        refreshEquipBtn.onClick.AddListener(RefreshCaptureEquip);
        closePopBtn.onClick.AddListener(HideInsufficientGoldPopUI);
        captureGoldBtn.onClick.AddListener(()=> { ShopManager.Instance.playerGlods.Increace(1000); });//临时获取金币
        nextLevelBtn.onClick.AddListener(()=> { GameLevelManager.Instance.CurrentLevelEnd(); });//临时切换下一关
        refreshEquipText.text = ShopManager.Instance.goldPrice.ToString();
    }

    public void ShowShopChoiceUI()
    {
        shopPanel.gameObject.SetActive(true);
        refreshEquipText.text = ShopManager.Instance.goldPrice.ToString();
        CaptureShopEquipUI();
    }
    public void HideShopChoiceUI()
    {
        shopPanel.gameObject.SetActive(false);
    }

    private void NextWave()
    {
        ShopManager.Instance.EndPurchase();
    }
    private void HideInsufficientGoldPopUI()
    {
        UIInsufficientGoldPop.gameObject.SetActive(false);
    }
    public void ShowInsufficientGoldPopUI()
    {
        UIInsufficientGoldPop.gameObject.SetActive(true);
    }

    public void ShowCurrentEquippedInfoCard(Equip equip)
    {
        currentEquippedInfoCard.Show(equip);
    }
    public void HideCurrentEquippedInfoCard()
    {
        currentEquippedInfoCard.Hide();
    }

    private void RefreshCaptureEquip()
    {
        if (ShopManager.Instance.playerGlods.CurrentValue >= ShopManager.Instance.goldPrice)
        {
            CaptureShopEquipUI();
            ShopManager.Instance.playerGlods.Decrease(ShopManager.Instance.goldPrice);
            ShopManager.Instance.goldPrice += 2;
            refreshEquipText.text = ShopManager.Instance.goldPrice.ToString();
        }
        else
        {
            ShopManager.Instance.ShowInsufficientGold();
        }
    }

    /// <summary>
    /// 获取五件装备显示到商店面板中（刷新按钮亦可用）
    /// </summary>
    private void CaptureShopEquipUI()
    {
        while (UISideEquipCapture.childCount > 0)
        {
            PutBack(UISideEquipCapture.GetChild(UISideEquipCapture.childCount - 1).gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            AddEquip(CaptureManager.Instance.GetCaptureEquip(GameLevelManager.Instance.GetCurrentLevel)) ;
        }
    }

    public void AddEquip(Equip equip)
    {
        UIShopEquipItem item;

        if (UIEquipItemPool.childCount > 0)
        {
            Transform t = UIEquipItemPool.GetChild(UIEquipItemPool.childCount - 1);
            item = t.GetComponent<UIShopEquipItem>();
            if (item)
            {
                item.Init(equip);
                t.parent = UISideEquipCapture;
                item.gameObject.SetActive(true);
            }
            else
            {
                Destroy(t.gameObject);
            }
        }
        else
        {
            GameObject go = Instantiate(EquipItem_Prefab, UISideEquipCapture);
            item = go.GetComponent<UIShopEquipItem>();
            item.Init(equip);
        }
    }

    public void PutBack(GameObject obj)
    {
        UIShopEquipItem item = obj.GetComponent<UIShopEquipItem>();
        if (item)
        {
            obj.SetActive(false);
            obj.transform.parent = UIEquipItemPool;
        }
        else
        {
            Destroy(obj);
        }
    }

}
