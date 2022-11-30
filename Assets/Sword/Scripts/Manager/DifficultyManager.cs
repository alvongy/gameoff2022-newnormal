using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : SerializedMonoSingleton<DifficultyManager>
{
    public DegreeOfDifficultyDatabase difficultyData;

    public int DifficultyID { get; set; }

    public DegreeOfDifficultySO CurrentDifficulty => difficultyData.database[DifficultyID];

    public int EnemyLevel => GameLevelManager.Instance.GetCurrentLevel;
    //Mathf.Min(totalEnemyLevel, (int)((Time.time - beginClock) / CurrentDifficulty.StrengthenEnemyCycleTime) + 1);

    public int CaptureLevel => EnemyLevel;
    //Mathf.Min(totalCaptureLevel, (int)((Time.time - beginClock) / CurrentDifficulty.StrengthenCaptureCycleTime) + 1);

    [HideInInspector] public float beginClock;

    [Title("EnemyLevel上限值"), SerializeField]
    public int totalEnemyLevel = 20;
    [Title("CaptureLevel上限值"), SerializeField]
    public int totalCaptureLevel = 20;
    [Title("刷怪梯度字典"), SerializeField]
    Dictionary<int, int> smoothDic;
    private int weightSum;
    private List<KeyValuePair<int, int>> list;

    protected override void OnAwake()
    {
        list = new List<KeyValuePair<int, int>>(8);
        weightSum = 0;
        foreach (var kv in smoothDic)
        {
            weightSum += kv.Value;
            list.Add(new KeyValuePair<int, int>(kv.Key, weightSum));
        }
    }

    public void Init()
    {
        DifficultyID = 1;
        beginClock = Time.time;
    }

    public int GetActualLevel(EnemySO so)
    {
        if(so.enemyTypeEnum== EnemyTypeEnum.General)
        {
            int n = Random.Range(0, weightSum);
            int result = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (n >= list[i].Value)
                {
                    break;
                }
                result = list[i].Key;
            }
            return Mathf.Clamp(EnemyLevel + result, 1, 20);
        }
        else
        {
            return EnemyLevel;
        }
    }
}
