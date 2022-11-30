using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Model/Enemy/Enemy")]
public class EnemySO : SerializedScriptableObject
{
    [Title("ID")]
    public int ID;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("����Ԥ��")]
    public GameObject Prefab;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("��ɫ��������")]
    public Dictionary<UpgradeAttribute, float> AttributeDic = default;
    [Title("װ������")]
    public int Probability;
    [Title("�ӵ�Ԥ����")]
    public GameObject projectilePrefab;
    [Title("��������")]
    public EnemyTypeEnum enemyTypeEnum;
}

public enum EnemyTypeEnum 
{
    None,
    General,
    Elite,
    Boss,
}
