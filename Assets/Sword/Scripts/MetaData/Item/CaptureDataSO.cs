using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptureDataSO", menuName = "Model/Capture/CaptureDataSO")]
public class CaptureDataSO : SerializedScriptableObject
{
    [Title("������-��׼ֵ��ȼ��仯���ձ�")]
    public Dictionary<UpgradeAttribute, float>[] firstAttributeValueOfLevelDic;
    [Title("�����Բ���ֵ")]
    public Dictionary<UpgradeAttribute, float>[] firstAttributeRangeDic;
    [Title("������-��׼ֵ��ȼ��仯���ձ�")]
    public Dictionary<UpgradeAttribute, float>[] secondAttributeValueOfLevelDic;
    [Title("�����Բ���ֵ")]
    public Dictionary<UpgradeAttribute, float>[] secondAttributeRangeDic;
    [Title("��������Ʒ����ȼ����ձ�")]
    public Dictionary<EquipQuality, float>[] productQualityDistribution;
    [Title("��������Ȩ����ȼ����ձ�")]
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