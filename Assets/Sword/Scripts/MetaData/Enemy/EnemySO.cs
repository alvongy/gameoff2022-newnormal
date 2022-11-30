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
    [Title("名称")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("敌人预制")]
    public GameObject Prefab;
    [Title("描述")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("角色基础属性")]
    public Dictionary<UpgradeAttribute, float> AttributeDic = default;
    [Title("装备爆率")]
    public int Probability;
    [Title("子弹预制体")]
    public GameObject projectilePrefab;
    [Title("敌人类型")]
    public EnemyTypeEnum enemyTypeEnum;
}

public enum EnemyTypeEnum 
{
    None,
    General,
    Elite,
    Boss,
}
