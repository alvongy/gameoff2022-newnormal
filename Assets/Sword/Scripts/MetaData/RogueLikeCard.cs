using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "RogueLikeCard", menuName = "RogueLikeCard/Roguelike Card")]
public class RogueLikeCard : SerializedScriptableObject
{
    [InfoBox("ID")]
    public int ID;
    [InfoBox("Ãû³Æ")]
    [DrawWithUnity]
    public LocalizedString Name;
    [InfoBox("Í¼±ê")]
    public Sprite Icon;
    [DrawWithUnity]
    [InfoBox("ÃèÊö")]
    public LocalizedString Description;
}
