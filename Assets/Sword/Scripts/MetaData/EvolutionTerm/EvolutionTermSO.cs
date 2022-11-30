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
    [Title("���� & ����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [DrawWithUnity]
    public LocalizedString Description;

    [Title("ͼ��")][HideLabel, PreviewField(55)]
    public Sprite Icon;

    [Title("����")]
    public int termNum;
    [Title("������������")]
    [DictionaryDrawerSettings(KeyLabel = "���ö���", ValueLabel = "���Դ���")]
    public Dictionary<TargetObjectEnum, Dictionary<UpgradeAttribute, float>> targetAttributeDic;

    [Title("�����������ͣ�����/����/���ԣ�")]
    public EvolutionTypeEnum evolutionType;
    [Title("��������Ʒ��")]
    public EquipQuality evolutionQuality;

    [Title("������������")]
    public SceneItemTypeEnum sceneItemType;
    [Title("�ؿ����º��Ƿ���������"), ShowIf("@(this.sceneItemType)!=SceneItemTypeEnum.NONE")]
    public bool isRebirth;
    [InfoBox("����Ԥ����"),ShowIf("@(this.isRebirth)==true")]
    public GameObject sceneItemPrefab;
    //public int sceneItemType;

    //[Title("װ��������Ȩ��")]
    //public Dictionary<UpgradeAttribute, float> WeightDic;
}

public enum EvolutionTypeEnum
{
    /// <summary>
    /// ����
    /// </summary>
    BENIGN,
    /// <summary>
    /// ����
    /// </summary>
    NEUTRAL,
    /// <summary>
    /// ����
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
    /// ˮ��
    /// </summary>
    MARSH,
    /// <summary>
    /// ����
    /// </summary>
    TRAP,
    /// <summary>
    /// ��Ȫ
    /// </summary>
    SPRING,
    /// <summary>
    /// ըҩͰ
    /// </summary>
    BANGALORE,
    /// <summary>
    /// ����
    /// </summary>
    TREASUREBOX,
    /// <summary>
    /// Ѫ��
    /// </summary>
    BLOOD,
    /// <summary>
    /// ������
    /// </summary>
    PORTAL,

}