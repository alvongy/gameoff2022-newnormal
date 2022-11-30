using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Model/Character/Character")]
public class CharacterSO : SerializedScriptableObject
{
    [Title("ID")]
    public int ID;
    [Title("名称")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("Prefab")]
    public GameObject Prefab;
    //[Title("图标")]
    //public Sprite Icon;
    [Title("描述")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("角色基础属性")]
    public Dictionary<UpgradeAttribute, float> AttributeDic = default;
    [Title("角色装备槽分布(Key:部位，Value:数量)")]
    public Dictionary<EquipPart, int> EquipPartDic = default;
}