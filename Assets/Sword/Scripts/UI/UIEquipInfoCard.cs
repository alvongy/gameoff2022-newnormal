using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipInfoCard : MonoBehaviour
{
    [SerializeField] CanvasGroup group;

    //[SerializeField] Image Border_Image;
    [SerializeField] Image Icon_Image;
    [SerializeField] Text Name_Text;
    [SerializeField] Text Part_Text;
    [SerializeField] Text Level_Text;
    [SerializeField] Text Quality_Text;
    [SerializeField] Text Score_Text;

    [SerializeField] Text[] MainAttributes;
    [SerializeField] Text[] SecondaryAttributes;
    [SerializeField] GameObject layoutGroup;

    public Equip equip;

    public void Show(Equip _equip)
    {
        equip = _equip;
        //Border_Image.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Icon_Image.sprite = equip.data.Icon;
        Name_Text.text = equip.data.Name.GetLocalizedString();
        Name_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Part_Text.text = equip.data.Part.GetLocalizedString();
        Level_Text.text = string.Format("Lv. {0}", equip.level);
        Level_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Quality_Text.text = CaptureManager.Instance.LocalizedMetaName_EquipQualityDic[equip.equipQuality].GetLocalizedString();
        Quality_Text.color = CaptureManager.Instance.QualityColorDic[equip.equipQuality];
        Score_Text.text = string.Format("{0}: {1}", CaptureManager.Instance.LocalizedMetaName_Score.GetLocalizedString(), equip.score);
        int i = 0;
        foreach(var kv in _equip.firstAttributeDic)
        {
            if (i < MainAttributes.Length)
            {
                if(kv.Key == UpgradeAttribute.AttackUpperLimit)
                {
                    i++;
                    continue;
                }
                if(kv.Key == UpgradeAttribute.AttackLowerLimit)
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
                    MainAttributes[i].text = CaptureManager.Instance.GetEquipAttributeFormat(kv.Key, (int)kv.Value);
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
        for(;i< MainAttributes.Length; i++)
        {
            MainAttributes[i].transform.parent.gameObject.SetActive(false);
        }
        i = 0;
        foreach (var kv in _equip.secondAttributeDic)
        {
            if (i < SecondaryAttributes.Length)
            {
                SecondaryAttributes[i].text = CaptureManager.Instance.GetEquipAttributeFormat(kv.Key, (int)kv.Value);
                SecondaryAttributes[i].transform.parent.gameObject.SetActive(true);
                i++;
            }
            else
            {
                Debug.LogError("UIEquipInfoCard.SecondaryAttributes PoolError: UI Item Is Not Enough");
                break;
            }
        }
        for(;i< SecondaryAttributes.Length; i++)
        {
            SecondaryAttributes[i].transform.parent.gameObject.SetActive(false);
        }
        group.alpha = 1;
        layoutGroup.SetActive(true);
    }

    public void ShowEmpty()
    {
        layoutGroup.SetActive(false);
        group.alpha = 1;
    }

    public void Hide()
    {
        equip = null;
        group.alpha = 0;
    }
}
