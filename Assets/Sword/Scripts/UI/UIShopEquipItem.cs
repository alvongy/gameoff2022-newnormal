using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopEquipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Alive => alive;
    private bool alive = false;
    [SerializeField] CanvasGroup group;

    [SerializeField] Image Border_Image;
    [SerializeField] Image Icon_Image;
    [SerializeField] Text Name_Text;
    [SerializeField] Text Part_Text;
    [SerializeField] Text Level_Text;
    [SerializeField] Text Score_Text;
    [SerializeField] Text Gold_Text;

    [SerializeField] Button BuyEquip_Button;
    
    [SerializeField] Text[] MainAttributes;
    [SerializeField] Text[] SecondaryAttributes;

    public Equip equip;

    public void Init(Equip _equip)
    {
        equip = _equip;
        Border_Image.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Icon_Image.sprite = equip.data.Icon;
        Name_Text.text = equip.data.Name.GetLocalizedString();
        Name_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Part_Text.text = equip.data.Part.GetLocalizedString();
        Level_Text.text = string.Format("Lv. {0}", equip.level);
        Level_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Score_Text.text = string.Format("{0}: {1}", CaptureManager.Instance.LocalizedMetaName_Score.GetLocalizedString(), equip.score);
        
        if (EvolutionManager.Instance.survivalDifficultyUnlockDic[3])
        {
            Gold_Text.text = ($"({equip.score / 2}) {equip.score * 0.4} G");
        }
        else
        {
            Gold_Text.text = string.Format("{0}{1}", equip.score / 2, "G");
        }

        BuyEquip_Button.onClick.AddListener(BuyEquip);
        
        int i = 0;
        foreach (var kv in _equip.firstAttributeDic)
        {
            if (i < MainAttributes.Length)
            {
                if (kv.Key == UpgradeAttribute.AttackUpperLimit)
                {
                    i++;
                    continue;
                }
                if (kv.Key == UpgradeAttribute.AttackLowerLimit)
                {
                    if (_equip.firstAttributeDic.ContainsKey(UpgradeAttribute.AttackUpperLimit))
                    {
                        MainAttributes[i].text = string.Format("{0}: {1:0}--{2:0}", CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[UpgradeAttribute.AttackLowerLimit].GetLocalizedString(), (int)kv.Value, (int)kv.Value + (int)_equip.firstAttributeDic[UpgradeAttribute.AttackUpperLimit]);
                        MainAttributes[i].transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        MainAttributes[i].text = string.Format("{0}: {1:0}", CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[UpgradeAttribute.AttackLowerLimit].GetLocalizedString(), (int)kv.Value);
                        MainAttributes[i].transform.parent.gameObject.SetActive(true);
                    }
                }
                else
                {
                    MainAttributes[i].text = CaptureManager.Instance.GetEquipAttributeFormat(kv.Key, kv.Value);
                    MainAttributes[i].transform.parent.gameObject.SetActive(true);
                }
                i++;
            }
            else
            {
                Debug.LogError("UIEquipInfoCard.MainAttributes PoolError: UI Item Is Not Enough");
                break;
            }
        }
        for (; i < MainAttributes.Length; i++)
        {
            MainAttributes[i].transform.parent.gameObject.SetActive(false);
        }
        i = 0;
        foreach (var kv in _equip.secondAttributeDic)
        {
            if (i < SecondaryAttributes.Length)
            {
                SecondaryAttributes[i].text = CaptureManager.Instance.GetEquipAttributeFormat(kv.Key, kv.Value);
                SecondaryAttributes[i].transform.parent.gameObject.SetActive(true);
                i++;
            }
            else
            {
                Debug.LogError("UIEquipInfoCard.SecondaryAttributes PoolError: UI Item Is Not Enough");
                break;
            }
        }
        for (; i < SecondaryAttributes.Length; i++)
        {
            SecondaryAttributes[i].transform.parent.gameObject.SetActive(false);
        }

        Show();
    }

    public void Hide()
    {
        group.alpha = 0;
        alive = false;
    }

    void Show()
    {
        alive = true;
        group.alpha = 1;
    }


    private void BuyEquip()
    {
        if (!alive) { return; }

        if (EvolutionManager.Instance.survivalDifficultyUnlockDic[3])
        {
            Gold_Text.text = ($"({equip.score / 2}) {equip.score * 0.4} G");
            Buy((int)(equip.score * 0.4));
        }
        else
        {
            Gold_Text.text = string.Format("{0}{1}", equip.score / 2, "G");
            Buy(equip.score / 2);
        }

        
    }
    private void Buy(int gold)
    {
        if (ShopManager.Instance.playerGlods.CurrentValue >= equip.score / 2)
        {
            if (CharacterManager.Instance.characterData.equipDictionary.ContainsKey(equip.data.PartCode))
            {
                ShopManager.Instance.playerGlods.Increace(CharacterManager.Instance.characterData.equipDictionary[equip.data.PartCode][0].score/4);

                CharacterManager.Instance.characterData.ArmEquip(equip, CharacterManager.Instance.characterData.equipDictionary[equip.data.PartCode][0]);
            }
            else
            {
                CharacterManager.Instance.characterData.ArmEquip(equip);
            }
            ShopManager.Instance.playerGlods.Decrease(equip.score / 2);
            Hide();
        }
        else
        {
            ShopManager.Instance.ShowInsufficientGold();
            Debug.Log("µ¯´°½ð±Ò²»×ã£¬¹ºÂòÊ§°Ü");
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!alive) { return; }
        ShopManager.Instance.CurrentEquipInfoCardUIControl(equip,true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopManager.Instance.CurrentEquipInfoCardUIControl(equip,false);
    }
}
