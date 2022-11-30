using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiePunishmentPanelDescription : MonoBehaviour
{
    public Text PunishmentDescription;

    public void Initialize(TerrainSO terrain)
    {
        PunishmentDescription.text = terrain.TerrainName.GetLocalizedString();
  
        //PrayFactions.text = YKDataInfoManager.Instance.AbilityDataBase.database[(int)item.DataPack["MFAbility"]].ablilityName.GetLocalizedString();
    }
}
