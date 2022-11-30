using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptureDataSO", menuName = "Model/Capture/CaptureDataSO")]
public class CaptureDataSO : SerializedScriptableObject
{
    [Title("主属性-基准值随等级变化对照表")]
    public Dictionary<UpgradeAttribute, float>[] firstAttributeValueOfLevelDic;
    [Title("主属性波动值")]
    public Dictionary<UpgradeAttribute, float>[] firstAttributeRangeDic;
    [Title("副属性-基准值随等级变化对照表")]
    public Dictionary<UpgradeAttribute, float>[] secondAttributeValueOfLevelDic;
    [Title("副属性波动值")]
    public Dictionary<UpgradeAttribute, float>[] secondAttributeRangeDic;
    [Title("各质量出品率随等级对照表")]
    public Dictionary<EquipQuality, float>[] productQualityDistribution;
    [Title("属性评分权重随等级对照表")]
    public Dictionary<UpgradeAttribute, float>[] scoreWeightDic;
}

public enum EquipQuality
{
    Dilapidated,
    Ordinary,
    HighQuality,
    Epic,
    Legendary,
    Mythical,
}

public enum EquipPart
{
    None,
    WeaponHeavy,
    WeaponB,
    WeaponC,
    WeaponD,
    WeaponE,
    WeaponF,
    Clothes,
    Shoes,
    Glove,
    Helmet,
    Ornaments,
    Cloak,
    Count,
}