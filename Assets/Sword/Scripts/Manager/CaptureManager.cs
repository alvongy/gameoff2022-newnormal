using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class CaptureManager : SerializedMonoSingleton<CaptureManager>//掉落相关
{
    [Title("UI本地化_评分")]
    [DrawWithUnity]
    public LocalizedString LocalizedMetaName_Score;
    [Title("UI本地化_获得")]
    [DrawWithUnity]
    public LocalizedString LocalizedMetaName_Capture;
    [Title("UI本地化_装备部位")]
    [DrawWithUnity]
    public LocalizedString[] LocalizedMetaName_EquipPart;
    [HideInInspector] public Dictionary<EquipPart, LocalizedString> LocalizedMetaName_EquipPartDic = default;
    [Title("UI装备品质-颜色字典")]
    public Dictionary<EquipQuality, Color> QualityColorDic = default;
    [Title("UI进化类型-颜色字典")]
    public Dictionary<EvolutionTypeEnum, Color> EvolutionTypeColorDic = default;

    [Title("UI本地化-装备品质")]
    [DrawWithUnity]
    public LocalizedString[] LocalizedMetaName_EquipQuality;
    [HideInInspector] public Dictionary<EquipQuality, LocalizedString> LocalizedMetaName_EquipQualityDic = default;
    [Title("UI本地化-装备属性")]
    [DrawWithUnity]
    public LocalizedString[] LocalizedMetaName_UpgradeAttribute;
    [HideInInspector] public Dictionary<UpgradeAttribute, LocalizedString> LocalizedMetaName_UpgradeAttributeDic = default;

    [HideInInspector] public List<EquipPart> list = default;
    List<EquipQuality> qualitiesList;
    List<float> randomWeightList;

    List<UpgradeAttribute> tempSecondAttributesList;
    List<int> tempSecondIndexList;

    List<Equip> equipPool;

    public string GetEquipAttributeFormat(UpgradeAttribute attribute, float value)
    {
        switch (attribute)
        {
            case UpgradeAttribute.TotalHp:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Recover:
                return string.Format("{0}: +{1:0.0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value/10);
            case UpgradeAttribute.AttackLowerLimit:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.AttackUpperLimit:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.MoveSpeed:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.AttackSpeed:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Defense:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Dodge:
                return string.Format("{0}: +{1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Vampire:
                return string.Format("{0}: +{1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.CriticalRate:
                return string.Format("{0}: +{1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.CriticalDamage:
                return string.Format("{0}: +{1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.PenetrateDamage:
                return string.Format("{0}: +{1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            default:
                return "";
        }
    }

    public string GetEntityAttributeFormat(UpgradeAttribute attribute, float value)
    {
        switch (attribute)
        {
            case UpgradeAttribute.TotalHp:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Recover:
                return string.Format("{0}: {1:0.0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value/10);
            case UpgradeAttribute.AttackLowerLimit:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.AttackUpperLimit:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.MoveSpeed:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.AttackSpeed:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Defense:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Dodge:
                return string.Format("{0}: {1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.Vampire:
                return string.Format("{0}: {1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.CriticalRate:
                return string.Format("{0}: {1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.CriticalDamage:
                return string.Format("{0}: {1:0}%", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            case UpgradeAttribute.PenetrateDamage:
                return string.Format("{0}: {1:0}", LocalizedMetaName_UpgradeAttributeDic[attribute].GetLocalizedString(), value);
            default:
                return "";
        }
    }

    protected override void OnAwake()
    {
        LocalizedMetaName_EquipPartDic = new Dictionary<EquipPart, LocalizedString>();
        for (EquipPart i = (EquipPart)1; i < EquipPart.Count; i++)
        {
            LocalizedMetaName_EquipPartDic.Add(i, LocalizedMetaName_EquipPart[(int)i]);
        }
        LocalizedMetaName_UpgradeAttributeDic = new Dictionary<UpgradeAttribute, LocalizedString>();
        for (UpgradeAttribute i = (UpgradeAttribute)1; i <= UpgradeAttribute.PenetrateDamage; i++)
        {
            LocalizedMetaName_UpgradeAttributeDic.Add(i, LocalizedMetaName_UpgradeAttribute[(int)i]);
        }
        LocalizedMetaName_EquipQualityDic = new Dictionary<EquipQuality, LocalizedString>();
        for (EquipQuality i = (EquipQuality)1; i <= EquipQuality.Legendary; i++)
        {
            LocalizedMetaName_EquipQualityDic.Add(i, LocalizedMetaName_EquipQuality[(int)i]);
        }
        equipPool = new List<Equip>(16);
    }

    public void Init(CharacterSO so)
    {
        qualitiesList = new List<EquipQuality>(8);
        randomWeightList = new List<float>();
        if (list != null)
        {
            list.Clear();
        }
        else
        {
            list = new List<EquipPart>(8);
        }
        var dic = so.EquipPartDic;
        foreach(var kv in dic)
        {
            if (kv.Value > 0)
            {
                list.Add(kv.Key);
            }
        }
        tempSecondAttributesList = new List<UpgradeAttribute>();
        tempSecondIndexList = new List<int>();
    }

    public EquipQuality GetEquipQuality(int level)
    {
        var productDic = DataManager.Instance.captureData.productQualityDistribution[level - 1];
        qualitiesList.Clear();
        randomWeightList.Clear();
        float lastValue = 0;
        foreach (var kv in productDic)
        {
            qualitiesList.Add(kv.Key);
            float t = kv.Value + lastValue;
            randomWeightList.Add(t);
            lastValue = t;
        }
        float v = Random.Range(0f, lastValue);
        EquipQuality result = qualitiesList[randomWeightList.Count - 1];
        for (int i = randomWeightList.Count - 1; i >= 0; i--)
        {
            if (v < randomWeightList[i])
            {
                result = qualitiesList[i];
            }
        }
        return result;
    }

    public bool CheckCapture(EntityData_Enemy enemy)
    {
        return Random.Range(0f, 1f) < enemy.probability;
    }

    public Equip GetCaptureEquip(int level)
    {
        var firstValueDic = DataManager.Instance.captureData.firstAttributeValueOfLevelDic[level - 1];
        var secondValueDic = DataManager.Instance.captureData.secondAttributeValueOfLevelDic[level - 1];
        var dic1 = DataManager.Instance.captureData.firstAttributeRangeDic[level - 1];
        var dic2 = DataManager.Instance.captureData.secondAttributeRangeDic[level - 1];

        EquipPart part = list[Random.Range(0, list.Count)];
        var equipList = DataManager.Instance.equipDatabase.partDic[part];
        EquipSO so = equipList[Random.Range(0, equipList.Count)];
        Dictionary<UpgradeAttribute, float> firstAttributeDic = new Dictionary<UpgradeAttribute, float>();
        foreach (var item in so.FirstAttributeList)
        {
            float f = Random.Range(-dic1[item], dic1[item]);
            firstAttributeDic.Add(item, (firstValueDic[item] + f) * so.WeightDic[item]);
            //Debug.LogFormat("主属性：{0}|等级：{1}|基础值：{2:0.00}|浮动范围：{3:0.0}|浮动结果：{4:0.00}|权重值：{5:0.0}|最终值：{6:0.00}"
            //    , LocalizedMetaName_UpgradeAttributeDic[item].GetLocalizedString(), level, firstValueDic[item]
            //    , dic1[item], f, so.WeightDic[item], firstAttributeDic[item]);
        }
        List<KeyValuePair<UpgradeAttribute, float>> secondAttributeDic = new List<KeyValuePair<UpgradeAttribute, float>>();
        EquipQuality quality = GetEquipQuality(level);
        int amount = (int)quality;
        if (amount > 0)
        {
            tempSecondAttributesList.Clear();
            tempSecondAttributesList.AddRange(so.SecondAttributeList);
            tempSecondIndexList.Clear();

            UpgradeAttribute result = 0;
            for (int i = 0; i < amount; i++)
            {
                if (tempSecondAttributesList.Count > tempSecondIndexList.Count)
                {
                    int index = Random.Range(0, tempSecondAttributesList.Count - tempSecondIndexList.Count);
                    for (int j = 0; j < tempSecondIndexList.Count; j++)
                    {
                        if (index >= tempSecondIndexList[j])
                        {
                            index++;
                        }
                    }
                    result = tempSecondAttributesList[index];
                    tempSecondIndexList.Add(index);
                    tempSecondIndexList.Sort();
                }
                else
                {
                    Debug.LogError("GetCaptureEquip Error: SecondAttributesList.Count is not enough!");
                }
                secondAttributeDic.Add(
                    new KeyValuePair<UpgradeAttribute, float>(result, Mathf.Max(1f
                    , (secondValueDic[result] + Random.Range(-dic2[result], dic2[result])) * so.WeightDic[result]))
                    );
            }
        }

        return CreateEquip(so, level, quality, firstAttributeDic, secondAttributeDic);
    }

    Equip CreateEquip(EquipSO so, int level, EquipQuality quality, Dictionary<UpgradeAttribute, float> firstDic, List<KeyValuePair<UpgradeAttribute, float>> secondDic)
    {
        Equip item;
        if (equipPool.Count > 0)
        {
            item = equipPool[equipPool.Count - 1];
            equipPool.RemoveAt(equipPool.Count - 1);
            item.Reset(so, level, quality, firstDic, secondDic);
        }
        else
        {
            item = Equip.GetEquip(so, level, quality, firstDic, secondDic);
        }
        return item;
    }

    public void DestroyEquip(Equip equip)
    {
        if (!equipPool.Contains(equip))
        {
            equipPool.Add(equip);
        }
    }
}
