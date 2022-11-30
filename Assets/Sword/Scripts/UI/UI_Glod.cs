using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Glod : MonoBehaviour
{
    [SerializeField] private Text _gold_Text;


    private void Update()
    {
        if (ShopManager.Instance.playerGlods != null)
        {
            _gold_Text.text = ShopManager.Instance.playerGlods.CurrentValue.ToString();
        }

    }
}
