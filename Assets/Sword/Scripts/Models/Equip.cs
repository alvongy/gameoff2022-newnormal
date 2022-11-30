using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Equip : MetaData
{
    public string MetaKey { get; set; }

    public EquipSO data;             //装备信息
    public int level;
    public EquipQuality equipQuality;
    public Dictionary<UpgradeAttribute, float> firstAttributeDic;  //主属性
    public List<KeyValuePair<UpgradeAttribute, float>> secondAttributeDic;  //副属性
    public Dictionary<UpgradeAttribute, float> resultAttributeDic;  //最终收益
    public int score;

    private Equip() { }

    public static Equip GetEquip(EquipSO so, int _level, EquipQuality quality, Dictionary<UpgradeAttribute, float> firstDic, List<KeyValuePair<UpgradeAttribute, float>> secondDic)
    {
        float scoreValue = 0;
        Equip equip = new Equip();
        equip.data = so;
        equip.level = _level;
        equip.equipQuality = quality;
        equip.firstAttributeDic = firstDic;
        equip.secondAttributeDic = secondDic;
        equip.resultAttributeDic = new Dictionary<UpgradeAttribute, float>();
        StringBuilder builder = new StringBuilder();
        builder.Append("ScoreLog："+equip.data.Name.GetLocalizedString() + "|Level:" + equip.level);
        foreach (var kv in equip.firstAttributeDic)
        {
            equip.resultAttributeDic.Add(kv.Key, kv.Value);
            int val = (int)kv.Value;
            float temp = DataManager.Instance.captureData.scoreWeightDic[equip.level - 1][kv.Key] * val;
            scoreValue += temp;
            builder.Append("|" + CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[kv.Key].GetLocalizedString() + ":" + DataManager.Instance.captureData.scoreWeightDic[equip.level - 1][kv.Key] + "*" + val + "=" + temp);
        }
        foreach(var item in equip.secondAttributeDic)
        {
            if (equip.resultAttributeDic.ContainsKey(item.Key))
            {
                equip.resultAttributeDic[item.Key] += item.Value;
            }
            else
            {
                equip.resultAttributeDic.Add(item.Key, item.Value);
            }
            int val = (int)item.Value;
            float temp = DataManager.Instance.captureData.scoreWeightDic[equip.level - 1][item.Key] * val;
            scoreValue += temp;
            builder.Append("|" + CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[item.Key].GetLocalizedString() + ":" + DataManager.Instance.captureData.scoreWeightDic[equip.level - 1][item.Key] + "*" + val + "=" + temp);
        }
        equip.score = (int)scoreValue;
        builder.Append("|<color=red>resultScore=" + equip.score + "</color>");
        //Debug.Log(builder.ToString());
        return equip;
    }

    public void Reset(EquipSO so, int _level, EquipQuality quality, Dictionary<UpgradeAttribute, float> firstDic, List<KeyValuePair<UpgradeAttribute, float>> secondDic)
    {
        float scoreValue = 0;
        data = so;
        level = _level;
        equipQuality = quality;
        firstAttributeDic = firstDic;
        secondAttributeDic = secondDic;
        resultAttributeDic.Clear();
        StringBuilder builder = new StringBuilder();
        builder.Append("ScoreLog：" + data.Name.GetLocalizedString() + "|Level:" + level);
        foreach (var kv in firstAttributeDic)
        {
            resultAttributeDic.Add(kv.Key, kv.Value);
            int val = (int)kv.Value;
            float temp = DataManager.Instance.captureData.scoreWeightDic[level - 1][kv.Key] * val;
            scoreValue += temp;
            builder.Append("|" + CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[kv.Key].GetLocalizedString() + ":" + DataManager.Instance.captureData.scoreWeightDic[level - 1][kv.Key] + "*" + val + "=" + temp);
        }
        foreach (var item in secondAttributeDic)
        {
            if (resultAttributeDic.ContainsKey(item.Key))
            {
                resultAttributeDic[item.Key] += item.Value;
            }
            else
            {
                resultAttributeDic.Add(item.Key, item.Value);
            }
            int val = (int)item.Value;
            float temp = DataManager.Instance.captureData.scoreWeightDic[level - 1][item.Key] * val;
            scoreValue += temp;
            builder.Append("|" + CaptureManager.Instance.LocalizedMetaName_UpgradeAttributeDic[item.Key].GetLocalizedString() + ":" + DataManager.Instance.captureData.scoreWeightDic[level - 1][item.Key] + "*" + val + "=" + temp);
            builder.Append("|<color=red>resultScore=" + score + "</color>");
            Debug.Log(builder.ToString());
        }
        score = (int)scoreValue;
    }
}
