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
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("Prefab")]
    public GameObject Prefab;
    //[Title("ͼ��")]
    //public Sprite Icon;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("��ɫ��������")]
    public Dictionary<UpgradeAttribute, float> AttributeDic = default;
    [Title("��ɫװ���۷ֲ�(Key:��λ��Value:����)")]
    public Dictionary<EquipPart, int> EquipPartDic = default;
}