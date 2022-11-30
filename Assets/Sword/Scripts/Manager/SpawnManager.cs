using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SerializedMonoSingleton<SpawnManager>
{
    [SerializeField] EnemyWaveDatabase database;

    public WaveSpawner_Sword[] WaveSpawner_Sword;

    protected override void OnAwake()
    {
        counter = int.MaxValue;
    }

    public bool Completed => counter == 0;
    public int counter;

    public void Init()
    {
        OnLevelEnter(GameLevelManager.Instance.spawnManagerConfigurationDic[1]);
    }

    public void OnLevelEnter(List<int> list)
    {
        if (list.Count > 0)
        {
            counter = list.Count;
            for (int i = 0; i < WaveSpawner_Sword.Length; i++)
            {
                WaveSpawner_Sword[i].loop = false;
                WaveSpawner_Sword[i].durationBetweenWaves = 2f;
                WaveSpawner_Sword[i].isStartRefreshEnemy = true;
                if (i < list.Count)
                {
                    Debug.Log("OnLevelEnter.AddListener");
                    WaveSpawner_Sword[i]._enemyWaveSO = database.waveDic[list[i]];
                    WaveSpawner_Sword[i].OnWaveEnd.AddListener(() =>
                    {
                        --counter;
                        Debug.Log("OnLevelEnter.counter:" + counter);
                    });
                    WaveSpawner_Sword[i].Init();
                }
            }
        }
    }

    public void OnGameOver()
    {
        StopWaveSpawner();
    }

    public void OnCurrentLevelEnd()
    {
        StopWaveSpawner();
    }

    public void StopWaveSpawner()
    {
        counter = int.MaxValue;
        for (int i = 0; i < WaveSpawner_Sword.Length; i++)
        {
            WaveSpawner_Sword[i].Stop();
        }
    }
}
