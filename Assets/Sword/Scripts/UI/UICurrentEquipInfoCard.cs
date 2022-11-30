using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICurrentEquipInfoCard : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private UIEquipInfoCard equippedInfoCard;

 public void Show(Equip equip)
    {
        canvasGroup.alpha = 1f;

        if (CharacterManager.Instance.characterData.equipDictionary.ContainsKey(equip.data.PartCode) 
            && CharacterManager.Instance.characterData.equipDictionary[equip.data.PartCode].Count > 0)
        {
            equippedInfoCard.Show(CharacterManager.Instance.characterData.equipDictionary[equip.data.PartCode][0]);
        }
        else
        {
            equippedInfoCard.ShowEmpty();
        }
  
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
