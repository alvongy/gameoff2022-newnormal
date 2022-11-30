using UnityEngine;
using UnityEngine.UI;

public class LearnAbilityEneity : MonoBehaviour
{
   public Text AbilityName;

    public void Initialize(int a)
    {
        AbilityName.text = YKDataInfoManager.Instance.AbilityDataBase.database[a].ablilityName.GetLocalizedString();
    }

}
