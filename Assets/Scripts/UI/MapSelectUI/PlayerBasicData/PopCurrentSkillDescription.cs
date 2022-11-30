using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopCurrentSkillDescription : MonoBehaviour
{
    public Text CurrentSkillDescription;

    public void Initialize(MFAbility ability)
    {
        CurrentSkillDescription.text = ability.Description.GetLocalizedString();
    }

}
