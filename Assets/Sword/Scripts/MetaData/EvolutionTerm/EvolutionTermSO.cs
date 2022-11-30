using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "EvolutionTermSO", menuName = "Model/Evolution/EvolutionTermSO")]
public class EvolutionTermSO : SerializedScriptableObject
{
    public string MetaKey { get; set; }

    [Title("ID")]
    public int ID;
    [Title("名称 & 描述")]
    [DrawWithUnity]
    public LocalizedString Name;
    [DrawWithUnity]
    public LocalizedString Description;

    [Title("图标")][HideLabel, PreviewField(55)]
    public Sprite Icon;

    [Title("数量")]
    public int termNum;
    [Title("进化属性配置")]
    [DictionaryDrawerSettings(KeyLabel = "作用对象", ValueLabel = "属性词条")]
    public Dictionary<TargetObjectEnum, Dictionary<UpgradeAttribute, float>> targetAttributeDic;

    [Title("进化词条类型（良性/中性/恶性）")]
    public EvolutionTypeEnum evolutionType;
    [Title("进化词条品质")]
    public EquipQuality evolutionQuality;

    [Title("场景道具类型")]
    public SceneItemTypeEnum sceneItemType;
    [Title("关卡更新后是否重新生成"), ShowIf("@(this.sceneItemType)!=SceneItemTypeEnum.NONE")]
    public bool isRebirth;
    [InfoBox("道具预制体"),ShowIf("@(this.isRebirth)==true")]
    public GameObject sceneItemPrefab;
    //public int sceneItemType;

    //[Title("装备的属性权重")]
    //public Dictionary<UpgradeAttribute, float> WeightDic;
}

public enum EvolutionTypeEnum
{
    /// <summary>
    /// 良性
    /// </summary>
    BENIGN,
    /// <summary>
    /// 中性
    /// </summary>
    NEUTRAL,
    /// <summary>
    /// 恶性
    /// </summary>
    MALIGNANCY,
}
public enum TargetObjectEnum
{
    PLAYER,
    ENEMYS
}
public enum SceneItemTypeEnum
{
    NONE,
    /// <summary>
    /// 水洼
    /// </summary>
    MARSH,
    /// <summary>
    /// 陷阱
    /// </summary>
    TRAP,
    /// <summary>
    /// 温泉
    /// </summary>
    SPRING,
    /// <summary>
    /// 炸药桶
    /// </summary>
    BANGALORE,
    /// <summary>
    /// 宝箱
    /// </summary>
    TREASUREBOX,
    /// <summary>
    /// 血包
    /// </summary>
    BLOOD,
    /// <summary>
    /// 传送门
    /// </summary>
    PORTAL,

}