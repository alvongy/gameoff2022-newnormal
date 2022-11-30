using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWS_EnemyHP : MonoBehaviour
{
    [SerializeField] Entity entity;

    [SerializeField] Image hpFill;
    [SerializeField] Text hp_Text;

    private void Update()
    {
        if (entity.Data != null)
        {
            hpFill.fillAmount = entity.Data.hp / (float)entity.Data.TotalHP;
            hp_Text.text = string.Format("{0}/{1}", entity.Data.hp, entity.Data.TotalHP);
        }
    }

}
