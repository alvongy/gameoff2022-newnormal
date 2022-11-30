using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YK.Game.Events;

public class MapPlayerBasicData : MonoBehaviour
{
    [FoldoutGroup("PlayerBasicData")][SerializeField] 
    public InventorySO _currentInventory = default;
    [FoldoutGroup("PlayerBasicData")][SerializeField] 
    private ItemEventChannelSO OnClickItemChannelSO;
    [FoldoutGroup("PlayerBasicData")]
    public Transform PrayControl;  
    [FoldoutGroup("PlayerBasicData")]
    public GameObject Pray;
    [FoldoutGroup("PlayerBasicData")]
    public Button CheakLearnAbility;

    public UnityAction OnClickPrayStoneEvent;

    private ItemSO currentItemSO;
    private Transform prayTransform;

    private void OnEnable()
    {
        OnClickItemChannelSO.OnEventRaised += GetItemSO;
        OnClickPrayStoneEvent += RefreshPrayStone;

        for (int i=0;i<PrayControl.childCount;i++)
        {
            Destroy(PrayControl.GetChild(i).gameObject);
        }
        foreach (var item in _currentInventory.Items)
        {
            GameObject prayGo= Instantiate(Pray, PrayControl);
            prayGo.GetComponent<PrayStoneEneity>().SetItem(item.Item);
            //设置祈祷石图标等信息
        }

       
    }
    private void Start()
    {
        CheakLearnAbility.onClick.AddListener(CheakAbility); 

        if (PrayControl.childCount == 0) { return; }
        currentItemSO = PrayControl.GetChild(0).GetComponent<PrayStoneEneity>().GetItem();
        OnClickItemChannelSO.RaiseEvent(currentItemSO);
        OnClickPrayStoneEvent?.Invoke();
    }
    private void OnDisable()
    {
        OnClickItemChannelSO.OnEventRaised -= GetItemSO;
        OnClickPrayStoneEvent -= RefreshPrayStone;
    }
    void GetItemSO(ItemSO item)
    {
        currentItemSO = item;
    }
    void RefreshPrayStone()
    {
        for (int i = 0; i < PrayControl.childCount; i++)
        {
            prayTransform = PrayControl.GetChild(i);
            if (prayTransform.GetComponent<PrayStoneEneity>().GetItem() == currentItemSO)
            {
                prayTransform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                prayTransform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    void CheakAbility()
    {
        MapSelectUIManager.Instance.PopUpPanel.InitializeLearnAbilityData();
    }
}
