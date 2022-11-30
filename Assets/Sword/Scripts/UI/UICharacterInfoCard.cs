using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfoCard : MonoBehaviour
{
    [SerializeField] Text[] attributeTextArray;
    [SerializeField] CanvasGroup canvasGroup;

    public void Init()
    {
        CharacterManager.Instance.characterData.OnDataChange += Refresh;
    }

    public void Refresh()
    {
        EntityData_Character data = CharacterManager.Instance.characterData;
        for (int i = 0; i < EntityData.allAttributes.Count; i++)
        {
            if (EntityData.allAttributes[i] == UpgradeAttribute.AttackUpperLimit|| EntityData.allAttributes[i] == UpgradeAttribute.EnemyBasicGold)
            {
                continue;
            }
            if (EntityData.allAttributes[i] == UpgradeAttribute.AttackLowerLimit)
            {
                attributeTextArray[i].text = string.Format("{0}: {1:0}--{2:0}", CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[UpgradeAttribute.AttackLowerLimit].GetLocalizedString(), (int)data[UpgradeAttribute.AttackLowerLimit].GetData(), (int)data[UpgradeAttribute.AttackLowerLimit].GetData()+(int)data[UpgradeAttribute.AttackUpperLimit].GetData());
            }
            else
            {
                attributeTextArray[i].text = CaptureManager.Instance.GetEntityAttributeFormat(EntityData.allAttributes[i], data[EntityData.allAttributes[i]].GetData());
            }
        }
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
