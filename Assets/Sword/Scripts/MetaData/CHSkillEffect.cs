using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public interface MetaData
{
    public string MetaKey { get; }
}

public abstract class CHSkillEffect : SerializedScriptableObject, MetaData
{
    public string MetaKey => GetType().ToString();

    //[InfoBox("ID")]
    //public int ID;
    //[InfoBox("����")]
    //[DrawWithUnity]
    //public LocalizedString Name;
    //[InfoBox("ͼ��")]
    //public Sprite Icon;
    //[DrawWithUnity]
    //[InfoBox("����")]
    //public LocalizedString Description;
}