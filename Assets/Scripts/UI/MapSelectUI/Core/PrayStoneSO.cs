using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum PrayStoneType
{
    Red, Green, Blue, Mix
}

[CreateAssetMenu(menuName = "Terrain/PrayStoneSO Entity")]
public class PrayStoneSO : SerializedScriptableObject
{
    [HideLabel, PreviewField(55)]
    public Sprite PrayStoneSprite;
    public int TID;
    [DrawWithUnity]
    public LocalizedString PrayStoneName;
    [DrawWithUnity]
    public LocalizedString PrayStoneDescription;

    public PrayStoneType PrayStonen_Type;

}
