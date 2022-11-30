using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PrayDescription : MonoBehaviour
{
    public Text PrayName;
    public Text PrayDes;
    public Text PrayFactions;

    public void Initialize(ItemSO item)
    {
        PrayName.text = item.Name.GetLocalizedString();
        PrayDes.text = item.Description.GetLocalizedString();
        PrayFactions.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].ablilityName.GetLocalizedString();
    }
}
