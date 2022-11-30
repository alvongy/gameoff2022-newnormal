using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Terrain/BossLanguageLibrary SO")]
public class BossLanguageLibrarySO : SerializedScriptableObject
{
    [TextArea]
    public string Description;
    [DrawWithUnity]
    public LocalizedString[] LanguageStrs;

}
