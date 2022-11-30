using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumericMetadata
{
    public UnityAction<float> OnValueChange = delegate { };

    /// <summary>
    /// 伤害什么的
    /// </summary>
    public float Data
    {
        get => data;
    }
    float data;

    /// <summary>
    /// 具体数值计算
    /// </summary>
    public float DataRatio
    {
        get => dataRatio;
    }
    float dataRatio;

    /// <summary>
    /// 加算值
    /// </summary>
    public float ExtraValue
    {
        get => extraValue;
    }
    float extraValue;

    Dictionary<string, float> ratioDic;
    Dictionary<string, float> extraValueDic;

    public NumericMetadata(float metadata)
    {
        data = metadata;
        dataRatio = 1;
        extraValue = 0;
        ratioDic = new Dictionary<string, float>();
        extraValueDic = new Dictionary<string, float>();
    }

    public NumericMetadata(float _metadata, float _dataRatio, float _extraValue)
    {
        data = _metadata;
        dataRatio = _dataRatio;
        extraValue = _extraValue;
        ratioDic = new Dictionary<string, float>();
        extraValueDic = new Dictionary<string, float>();
    }

    public NumericMetadata Copy()
    {
        return new NumericMetadata(data, dataRatio, extraValue);
    }

    public void Reset(float _metadata, float _dataRatio, float _extraValue)
    {
        data = _metadata;
        dataRatio = _dataRatio;
        extraValue = _extraValue;
        ratioDic.Clear();
        extraValueDic.Clear();
        OnValueChange = delegate { };
    }

    public void SetRatioDic(string effect, float value)
    {
        if (ratioDic.ContainsKey(effect))
        {
            dataRatio += value - ratioDic[effect];
            ratioDic[effect] = value;
        }
        else
        {
            dataRatio += value;
            ratioDic.Add(effect, value);
        }
    }

    public float GetExtraValue(string effect)
    {
        if (extraValueDic.ContainsKey(effect))
        {
            return extraValueDic[effect];
        }
        else
        {
            return 0;
        }
    }

    public void SetExtraValueDic(string effect, float value)
    {
        if (extraValueDic.ContainsKey(effect))
        {
            extraValue += value - extraValueDic[effect];
            extraValueDic[effect] = value;
        }
        else
        {
            extraValue += value;
            extraValueDic.Add(effect, value);
        }
    }

    public void RemoveEffect(string effect)
    {
        if (ratioDic.ContainsKey(effect))
        {
            dataRatio -= ratioDic[effect];
            ratioDic.Remove(effect);
        }
        if (extraValueDic.ContainsKey(effect))
        {
            extraValue -= extraValueDic[effect];
            extraValueDic.Remove(effect);
        }
    }

    public float GetData()
    {
        return (Data * DataRatio) + ExtraValue;
    }
}
