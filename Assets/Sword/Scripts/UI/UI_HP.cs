using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP : MonoBehaviour
{
    [SerializeField] Image HP_Image;
    [SerializeField] Text HP_Text;

    private void Update()
    {
        if (CharacterManager.Instance.characterData != null)
        {
            HP_Image.fillAmount = CharacterManager.Instance.characterData.hp / (float)CharacterManager.Instance.characterData.TotalHP;
            HP_Text.text = string.Format("{0}/{1}", CharacterManager.Instance.characterData.hp, CharacterManager.Instance.characterData.TotalHP);
        }
    }
}
