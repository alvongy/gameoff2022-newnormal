using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SkillCardEneity : MonoBehaviour,IPointerDownHandler
{
	public int SkillID;
	public Text SkillName;
	public Text SkillDescription;
	public Image SkillImage;

	[SerializeField] private IntEventChannelSO _addSecletcSkillEvent;

	private int currentLevel;
	public void SkillCardInit(MFAbility ability)
    {
		//currentLevel = (int)ability.DataPack[MFAbility.AbilityDataUpgrade.UpgradeType.Level].GetData();
		currentLevel = 0;
		//currentLevel = ability.level;
		SkillID = ability.AID;
		SkillName.text = ability.ablilityName.GetLocalizedString();
		SkillDescription.text = ability.Description.GetLocalizedString();
		SkillImage.sprite = ability.Front;

        switch (currentLevel)
        {
            case 0:
				SkillDescription.text = ability.Description.GetLocalizedString();
				break;
			default:
				SkillDescription.text = ability.AbilityLevelUpLibrary.AbilityLevelUpStrs[currentLevel-1].GetLocalizedString();
				break;
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_addSecletcSkillEvent.RaiseEvent(SkillID);
	}
}
