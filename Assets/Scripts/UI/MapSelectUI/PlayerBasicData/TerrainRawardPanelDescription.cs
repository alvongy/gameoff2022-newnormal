using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainRawardPanelDescription : MonoBehaviour
{
    public Text TerrainRawardDescription;

    public void Initialize(YKRewardSO reward)
    {
        TerrainRawardDescription.text = reward.RewardName.GetLocalizedString();
        //PrayFactions.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].ablilityName.GetLocalizedString();
    }
}
