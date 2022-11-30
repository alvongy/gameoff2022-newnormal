using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDescription : MonoBehaviour
{
    public Text PrayName;
    public Text PrayDes;
    public Text PrayFactions;

    public void Initialize(ItemSO item)
    {
        PrayName.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].ablilityName.GetLocalizedString();
        PrayDes.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].Description.GetLocalizedString();
        //PrayFactions.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].ablilityName.GetLocalizedString();
    }
}
