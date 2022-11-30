using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "EquipSO", menuName = "Model/Equip/Equip")]
public class EquipSO : SerializedScriptableObject
{
    [Title("ID")]
    public int ID;
    [Title("部位")]
    public EquipPart PartCode;
    [Title("名称")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("部位")]
    [DrawWithUnity]
    public LocalizedString Part;
    [Title("图标")]
    public Sprite Icon;
    [Title("描述")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("装备的主属性")]
    public List<UpgradeAttribute> FirstAttributeList;
    [Title("装备的副属性")]
    public List<UpgradeAttribute> SecondAttributeList;
    [Title("装备的属性权重")]
    public Dictionary<UpgradeAttribute, float> WeightDic;

}
