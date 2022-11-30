using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "MFAbility/AbilityLevelUpLibrary SO ")]
public class AbilityLevelUpLibrarySO : SerializedScriptableObject
{
    [TextArea]
    public string Description;
    [DrawWithUnity]
    public LocalizedString[] AbilityLevelUpStrs;
}
