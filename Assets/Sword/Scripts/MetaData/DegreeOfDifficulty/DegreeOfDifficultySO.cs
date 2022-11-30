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
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Name;
    [Title("����")]
    [DrawWithUnity]
    public LocalizedString Description;
    [Title("Round")]
    public float Round;
    [Title("Difficult")]
    public float Difficult;
    [Title("���˱�ǿ����(s)")]
    public float StrengthenEnemyCycleTime;
    [Title("���佱����ǿ����(s)")]
    public float StrengthenCaptureCycleTime;
}
