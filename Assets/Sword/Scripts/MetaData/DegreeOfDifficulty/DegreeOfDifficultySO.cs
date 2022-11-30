using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DegreeOfDifficulty", menuName = "Model/DegreeOfDifficulty/DegreeOfDifficulty")]
public class DegreeOfDifficultySO : SerializedScriptableObject
{
    [Title("ID")]
    public int ID;
    [Title("名称")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("描述")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("Round")]
    public float Round;
    [Title("Difficult")]
    public float Difficult;
    [Title("敌人变强周期(s)")]
    public float StrengthenEnemyCycleTime;
    [Title("掉落奖励变强周期(s)")]
    public float StrengthenCaptureCycleTime;
}
