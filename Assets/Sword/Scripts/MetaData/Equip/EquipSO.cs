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
    [Title("��λ")]
    public EquipPart PartCode;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("��λ")]
    [DrawWithUnity]
    public LocalizedString Part;
    [Title("ͼ��")]
    public Sprite Icon;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("װ����������")]
    public List<UpgradeAttribute> FirstAttributeList;
    [Title("װ���ĸ�����")]
    public List<UpgradeAttribute> SecondAttributeList;
    [Title("װ��������Ȩ��")]
    public Dictionary<UpgradeAttribute, float> WeightDic;

}
