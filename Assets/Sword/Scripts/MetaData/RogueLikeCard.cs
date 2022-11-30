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
    [InfoBox("����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [InfoBox("ͼ��")]
    public Sprite Icon;
    [DrawWithUnity]
    [InfoBox("����")]
    public LocalizedString Description;
}
